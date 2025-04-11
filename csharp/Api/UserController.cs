using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XaubotClone.Domain;
using XaubotClone.Services;

namespace XaubotClone.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var userDtos = users.Select(MapToDto).ToList();
                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                
                if (user == null)
                    return NotFound($"User with ID {id} not found");
                    
                return Ok(MapToDto(user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            try
            {
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Role = request.Role ?? UserRole.Regular
                };
                
                var userId = await _userService.CreateUserAsync(user, request.Password);
                
                return CreatedAtAction(nameof(GetUserById), new { id = userId }, MapToDto(user));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                
                if (user == null)
                    return NotFound($"User with ID {id} not found");
                    
                // Update user properties
                user.Username = request.Username ?? user.Username;
                user.Email = request.Email ?? user.Email;
                user.FirstName = request.FirstName ?? user.FirstName;
                user.LastName = request.LastName ?? user.LastName;
                
                if (request.Role.HasValue)
                    user.Role = request.Role.Value;
                    
                await _userService.UpdateUserAsync(user);
                
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                
                if (!result)
                    return NotFound($"User with ID {id} not found");
                    
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            try
            {
                var isValid = await _userService.ValidateUserCredentialsAsync(request.UsernameOrEmail, request.Password);
                
                if (!isValid)
                    return Unauthorized("Invalid username/email or password");
                    
                // Get user data
                var user = await _userService.GetUserByUsernameAsync(request.UsernameOrEmail);
                
                if (user == null)
                    user = await _userService.GetUserByEmailAsync(request.UsernameOrEmail);
                    
                // In a real application, you would generate a JWT token here
                return Ok(new { 
                    Message = "Authentication successful",
                    UserId = user.Id,
                    Username = user.Username,
                    Role = user.Role.ToString()
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            try
            {
                var result = await _userService.ChangePasswordAsync(
                    request.UserId, 
                    request.CurrentPassword, 
                    request.NewPassword);
                    
                if (!result)
                    return BadRequest("Invalid current password");
                    
                return Ok(new { Message = "Password changed successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Helper method to map User to UserDto
        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }
    }

    // DTOs
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole? Role { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole? Role { get; set; }
    }

    public class AuthenticateRequest
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }

    public class ChangePasswordRequest
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
