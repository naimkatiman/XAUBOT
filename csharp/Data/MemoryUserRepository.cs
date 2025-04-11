using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XaubotClone.Domain;

namespace XaubotClone.Data
{
    public class MemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>
        {
            new User 
            { 
                Id = 1, 
                Username = "admin", 
                Email = "admin@xaubot.com", 
                PasswordHash = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", // admin123
                FirstName = "Admin", 
                LastName = "User", 
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                LastLoginAt = DateTime.UtcNow.AddDays(-1),
                IsActive = true
            },
            new User 
            { 
                Id = 2, 
                Username = "trader1", 
                Email = "trader1@example.com", 
                PasswordHash = "6Uu4zgpSvkMp1URQ5IUGHBJpEnNQjyKPqrK4KEbLmTw=", // trader123
                FirstName = "John", 
                LastName = "Doe", 
                Role = UserRole.Regular,
                CreatedAt = DateTime.UtcNow.AddDays(-20),
                LastLoginAt = DateTime.UtcNow.AddDays(-2),
                IsActive = true
            },
            new User 
            { 
                Id = 3, 
                Username = "premium", 
                Email = "premium@example.com", 
                PasswordHash = "YxmQD3RDT3C8xEBiqnL6CUrPZ30y0JeO8WthB6JoFxo=", // premium123
                FirstName = "Jane", 
                LastName = "Smith", 
                Role = UserRole.Premium,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                LastLoginAt = DateTime.UtcNow.AddHours(-12),
                IsActive = true
            }
        };

        private readonly object _lock = new object();
        private int _nextId = 4;

        public Task<List<User>> GetAllAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_users.ToList());
            }
        }

        public Task<User> GetByIdAsync(int id)
        {
            lock (_lock)
            {
                var user = _users.FirstOrDefault(u => u.Id == id);
                return Task.FromResult(user);
            }
        }

        public Task<User> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty");

            lock (_lock)
            {
                var user = _users.FirstOrDefault(u => 
                    u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                    
                return Task.FromResult(user);
            }
        }

        public Task<User> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty");
                
            lock (_lock)
            {
                var user = _users.FirstOrDefault(u => 
                    u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                    
                return Task.FromResult(user);
            }
        }

        public Task<int> AddAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
                
            lock (_lock)
            {
                // Check for existing username or email
                if (_users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException($"Username '{user.Username}' is already taken");
                    
                if (_users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException($"Email '{user.Email}' is already registered");
                
                // Assign ID
                user.Id = _nextId++;
                
                // Add to collection
                _users.Add(user);
                
                return Task.FromResult(user.Id);
            }
        }

        public Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
                
            lock (_lock)
            {
                // Find user in collection
                var index = _users.FindIndex(u => u.Id == user.Id);
                
                if (index < 0)
                    throw new ArgumentException($"User with ID {user.Id} not found");
                    
                // Check for username uniqueness (excluding current user)
                if (_users.Any(u => u.Id != user.Id && 
                    u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException($"Username '{user.Username}' is already taken");
                    
                // Check for email uniqueness (excluding current user)
                if (_users.Any(u => u.Id != user.Id && 
                    u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException($"Email '{user.Email}' is already registered");
                
                // Update user in collection
                _users[index] = user;
                
                return Task.CompletedTask;
            }
        }

        public Task DeleteAsync(int id)
        {
            lock (_lock)
            {
                // Find user in collection
                var index = _users.FindIndex(u => u.Id == id);
                
                if (index < 0)
                    throw new ArgumentException($"User with ID {id} not found");
                    
                // Remove from collection
                _users.RemoveAt(index);
                
                return Task.CompletedTask;
            }
        }
    }
}
