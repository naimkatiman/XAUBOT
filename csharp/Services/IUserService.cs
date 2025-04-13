using System.Collections.Generic;
using System.Threading.Tasks;
using XaubotClone.Domain;

namespace XaubotClone.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> AuthenticateUserAsync(string username, string password);
        Task<int> RegisterUserAsync(User user, string password);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> SetUserPreferenceAsync(int userId, string key, string value);
        Task<string> GetUserPreferenceAsync(int userId, string key, string defaultValue = null);
    }
}
