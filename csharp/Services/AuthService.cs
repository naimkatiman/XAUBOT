using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using XaubotClone.Data;
using XaubotClone.Domain;

namespace XaubotClone.Services
{
    public interface IAuthService
    {
        Task<AuthResult> AuthenticateAsync(string usernameOrEmail, string password);
        Task<AuthResult> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
        Task<User> GetUserFromTokenAsync(string token);
        Task<string> GeneratePasswordResetTokenAsync(int userId);
        Task<bool> ValidatePasswordResetTokenAsync(string token, int userId);
        Task<bool> ResetPasswordAsync(int userId, string token, string newPassword);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<string> HashPasswordAsync(string password);
        Task<bool> VerifyPasswordAsync(string password, string hash);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _tokenLifetime;
        private readonly TimeSpan _refreshTokenLifetime;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
            // Read token lifetimes from configuration, or use defaults
            _tokenLifetime = TimeSpan.FromMinutes(
                configuration.GetValue<int>("JwtSettings:TokenLifetimeMinutes", 30));
            
            _refreshTokenLifetime = TimeSpan.FromDays(
                configuration.GetValue<int>("JwtSettings:RefreshTokenLifetimeDays", 30));
        }

        public async Task<AuthResult> AuthenticateAsync(string usernameOrEmail, string password)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(usernameOrEmail))
                throw new ArgumentException("Username or email cannot be empty");
                
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty");
                
            // Try to find user by username or email
            var user = await _userRepository.GetByUsernameAsync(usernameOrEmail);
            
            if (user == null)
                user = await _userRepository.GetByEmailAsync(usernameOrEmail);
                
            if (user == null || !user.IsActive)
                return new AuthResult { Success = false, ErrorMessage = "Invalid username/email or password" };
                
            // Verify password
            bool isPasswordValid = await VerifyPasswordAsync(password, user.PasswordHash);
            
            if (!isPasswordValid)
                return new AuthResult { Success = false, ErrorMessage = "Invalid username/email or password" };
                
            // Update last login timestamp
            user.UpdateLastLogin();
            await _userRepository.UpdateAsync(user);
            
            // Generate tokens
            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            
            // Store refresh token in user preferences
            user.SetPreference("RefreshToken", refreshToken);
            user.SetPreference("RefreshTokenExpiry", DateTime.UtcNow.Add(_refreshTokenLifetime).ToString("o"));
            await _userRepository.UpdateAsync(user);
            
            return new AuthResult
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.Add(_tokenLifetime),
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return new AuthResult { Success = false, ErrorMessage = "Refresh token is required" };
                
            // Get all users and find the one with matching refresh token
            var users = await _userRepository.GetAllAsync();
            var user = users.Find(u => u.GetPreference("RefreshToken") == refreshToken);
            
            if (user == null)
                return new AuthResult { Success = false, ErrorMessage = "Invalid refresh token" };
                
            // Check if token is expired
            var expiryString = user.GetPreference("RefreshTokenExpiry");
            if (string.IsNullOrEmpty(expiryString) || 
                !DateTime.TryParse(expiryString, out var expiry) ||
                expiry < DateTime.UtcNow)
            {
                return new AuthResult { Success = false, ErrorMessage = "Refresh token has expired" };
            }
            
            // Generate new tokens
            var accessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();
            
            // Update refresh token in user preferences
            user.SetPreference("RefreshToken", newRefreshToken);
            user.SetPreference("RefreshTokenExpiry", DateTime.UtcNow.Add(_refreshTokenLifetime).ToString("o"));
            await _userRepository.UpdateAsync(user);
            
            return new AuthResult
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.Add(_tokenLifetime),
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return false;
                
            // Get all users and find the one with matching refresh token
            var users = await _userRepository.GetAllAsync();
            var user = users.Find(u => u.GetPreference("RefreshToken") == refreshToken);
            
            if (user == null)
                return false;
                
            // Remove refresh token from user preferences
            user.SetPreference("RefreshToken", null);
            user.SetPreference("RefreshTokenExpiry", null);
            await _userRepository.UpdateAsync(user);
            
            return true;
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult(false);
                
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public async Task<User> GetUserFromTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;
                
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                    return null;
                    
                return await _userRepository.GetByIdAsync(userId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");
                
            // Generate a random token
            var token = GenerateRandomToken();
            
            // Store token in user preferences with expiry (24 hours)
            user.SetPreference("PasswordResetToken", token);
            user.SetPreference("PasswordResetTokenExpiry", DateTime.UtcNow.AddHours(24).ToString("o"));
            await _userRepository.UpdateAsync(user);
            
            return token;
        }

        public async Task<bool> ValidatePasswordResetTokenAsync(string token, int userId)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;
                
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;
                
            // Get token from user preferences
            var storedToken = user.GetPreference("PasswordResetToken");
            var expiryString = user.GetPreference("PasswordResetTokenExpiry");
            
            if (string.IsNullOrEmpty(storedToken) || string.IsNullOrEmpty(expiryString))
                return false;
                
            // Check if token matches and is not expired
            return token == storedToken && 
                   DateTime.TryParse(expiryString, out var expiry) &&
                   expiry > DateTime.UtcNow;
        }

        public async Task<bool> ResetPasswordAsync(int userId, string token, string newPassword)
        {
            // Validate token
            bool isValidToken = await ValidatePasswordResetTokenAsync(token, userId);
            if (!isValidToken)
                return false;
                
            // Get user
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;
                
            // Hash new password
            user.PasswordHash = await HashPasswordAsync(newPassword);
            
            // Clear reset token
            user.SetPreference("PasswordResetToken", null);
            user.SetPreference("PasswordResetTokenExpiry", null);
            
            // Update user
            await _userRepository.UpdateAsync(user);
            
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");
                
            // Verify current password
            bool isCurrentPasswordValid = await VerifyPasswordAsync(currentPassword, user.PasswordHash);
            if (!isCurrentPasswordValid)
                return false;
                
            // Hash new password
            user.PasswordHash = await HashPasswordAsync(newPassword);
            
            // Update user
            await _userRepository.UpdateAsync(user);
            
            return true;
        }

        public Task<string> HashPasswordAsync(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty");
                
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Task.FromResult(Convert.ToBase64String(hashedBytes));
            }
        }

        public Task<bool> VerifyPasswordAsync(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
                return Task.FromResult(false);
                
            string hashedPassword = null;
            
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                hashedPassword = Convert.ToBase64String(hashedBytes);
            }
            
            return Task.FromResult(hashedPassword == hash);
        }

        // Helper methods
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_tokenLifetime),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateRandomToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string ErrorMessage { get; set; }
    }
}
