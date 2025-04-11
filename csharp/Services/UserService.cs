using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XaubotClone.Data;
using XaubotClone.Domain;

namespace XaubotClone.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllUsersAsync();
        Task<int> CreateUserAsync(User user, string password);
        Task UpdateUserAsync(User user);
        Task<bool> ValidateUserCredentialsAsync(string usernameOrEmail, string password);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> DeleteUserAsync(int id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        
        private static readonly List<User> _mockUsers = new List<User>
        {
            new User 
            { 
                Id = 1, 
                Username = "admin", 
                Email = "admin@xaubot.com", 
                PasswordHash = HashPassword("admin123"), 
                FirstName = "Admin", 
                LastName = "User", 
                Role = UserRole.Admin 
            },
            new User 
            { 
                Id = 2, 
                Username = "trader1", 
                Email = "trader1@example.com", 
                PasswordHash = HashPassword("trader123"), 
                FirstName = "John", 
                LastName = "Doe", 
                Role = UserRole.Regular 
            },
            new User 
            { 
                Id = 3, 
                Username = "premium", 
                Email = "premium@example.com", 
                PasswordHash = HashPassword("premium123"), 
                FirstName = "Jane", 
                LastName = "Smith", 
                Role = UserRole.Premium 
            }
        };

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty");
                
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty");
                
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<int> CreateUserAsync(User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
                
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty");
                
            // Validate user data
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username cannot be empty");
                
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email cannot be empty");
                
            // Check if username or email already exists
            if (await _userRepository.GetByUsernameAsync(user.Username) != null)
                throw new InvalidOperationException($"Username '{user.Username}' is already taken");
                
            if (await _userRepository.GetByEmailAsync(user.Email) != null)
                throw new InvalidOperationException($"Email '{user.Email}' is already registered");
                
            // Hash password
            user.PasswordHash = HashPassword(password);
            
            // Set default values
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;
            
            // Create user
            return await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
                
            // Validate user data
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username cannot be empty");
                
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email cannot be empty");
                
            // Check if username or email already exists (excluding current user)
            var existingUsername = await _userRepository.GetByUsernameAsync(user.Username);
            if (existingUsername != null && existingUsername.Id != user.Id)
                throw new InvalidOperationException($"Username '{user.Username}' is already taken");
                
            var existingEmail = await _userRepository.GetByEmailAsync(user.Email);
            if (existingEmail != null && existingEmail.Id != user.Id)
                throw new InvalidOperationException($"Email '{user.Email}' is already registered");
                
            await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> ValidateUserCredentialsAsync(string usernameOrEmail, string password)
        {
            if (string.IsNullOrWhiteSpace(usernameOrEmail))
                throw new ArgumentException("Username or email cannot be empty");
                
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty");
                
            // Try to find user by username or email
            var user = await _userRepository.GetByUsernameAsync(usernameOrEmail);
            
            if (user == null)
                user = await _userRepository.GetByEmailAsync(usernameOrEmail);
                
            if (user == null || !user.IsActive)
                return false;
                
            // Verify password
            bool isValid = VerifyPassword(password, user.PasswordHash);
            
            if (isValid)
            {
                // Update last login timestamp
                user.UpdateLastLogin();
                await _userRepository.UpdateAsync(user);
            }
            
            return isValid;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword))
                throw new ArgumentException("Current password cannot be empty");
                
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("New password cannot be empty");
                
            var user = await _userRepository.GetByIdAsync(userId);
            
            if (user == null || !user.IsActive)
                return false;
                
            // Verify current password
            if (!VerifyPassword(currentPassword, user.PasswordHash))
                return false;
                
            // Update password
            user.PasswordHash = HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);
            
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            
            if (user == null)
                return false;
                
            // Soft delete (deactivate) the user
            user.Deactivate();
            await _userRepository.UpdateAsync(user);
            
            return true;
        }
        
        // Password hashing utilities
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
        
        private static bool VerifyPassword(string password, string passwordHash)
        {
            string hashedPassword = HashPassword(password);
            return hashedPassword == passwordHash;
        }
    }
}
