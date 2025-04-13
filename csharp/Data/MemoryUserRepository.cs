using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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
            },
            new User 
            { 
                Id = 4, 
                Username = "trader2", 
                Email = "trader2@example.com", 
                PasswordHash = "8nPswxXhH29w6EEQWQTf2U9ZZ3zRJLN8XNsU4aDuQRE=", // trader456
                FirstName = "Robert", 
                LastName = "Johnson", 
                Role = UserRole.Regular,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                LastLoginAt = DateTime.UtcNow.AddHours(-2),
                IsActive = true
            }
        };

        private readonly object _lock = new object();
        private int _nextId = 5;
        
        // Audit log to track changes to users
        private readonly List<UserAuditLog> _auditLogs = new List<UserAuditLog>();

        public Task<List<User>> GetAllAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_users.ToList());
            }
        }
        
        // Pagination methods
        public Task<PagedResult<User>> GetPagedUsersAsync(int pageNumber, int pageSize, UserFilterCriteria filterCriteria = null)
        {
            if (pageNumber < 1) 
                throw new ArgumentException("Page number must be greater than or equal to 1");
                
            if (pageSize < 1) 
                throw new ArgumentException("Page size must be greater than or equal to 1");
                
            lock (_lock)
            {
                // Start with all users
                IQueryable<User> query = _users.AsQueryable();
                
                // Apply filtering if criteria provided
                if (filterCriteria != null)
                {
                    // Search term (username, email, first name, last name)
                    if (!string.IsNullOrWhiteSpace(filterCriteria.SearchTerm))
                    {
                        query = query.Where(u => 
                            u.Username.Contains(filterCriteria.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                            u.Email.Contains(filterCriteria.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                            (u.FirstName != null && u.FirstName.Contains(filterCriteria.SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                            (u.LastName != null && u.LastName.Contains(filterCriteria.SearchTerm, StringComparison.OrdinalIgnoreCase)));
                    }
                    
                    // Filter by role
                    if (filterCriteria.Role.HasValue)
                    {
                        query = query.Where(u => u.Role == filterCriteria.Role.Value);
                    }
                    
                    // Filter by active status
                    if (filterCriteria.IsActive.HasValue)
                    {
                        query = query.Where(u => u.IsActive == filterCriteria.IsActive.Value);
                    }
                    
                    // Filter by created date range
                    if (filterCriteria.CreatedAfter.HasValue)
                    {
                        query = query.Where(u => u.CreatedAt >= filterCriteria.CreatedAfter.Value);
                    }
                    
                    if (filterCriteria.CreatedBefore.HasValue)
                    {
                        query = query.Where(u => u.CreatedAt <= filterCriteria.CreatedBefore.Value);
                    }
                    
                    // Filter by last login date range
                    if (filterCriteria.LastLoginAfter.HasValue)
                    {
                        query = query.Where(u => u.LastLoginAt.HasValue && u.LastLoginAt.Value >= filterCriteria.LastLoginAfter.Value);
                    }
                    
                    if (filterCriteria.LastLoginBefore.HasValue)
                    {
                        query = query.Where(u => u.LastLoginAt.HasValue && u.LastLoginAt.Value <= filterCriteria.LastLoginBefore.Value);
                    }
                    
                    // Apply sorting
                    if (!string.IsNullOrWhiteSpace(filterCriteria.SortBy))
                    {
                        // Apply ordering based on the property name
                        switch (filterCriteria.SortBy.ToLowerInvariant())
                        {
                            case "username":
                                query = filterCriteria.SortDescending ? 
                                    query.OrderByDescending(u => u.Username) : 
                                    query.OrderBy(u => u.Username);
                                break;
                            case "email":
                                query = filterCriteria.SortDescending ? 
                                    query.OrderByDescending(u => u.Email) : 
                                    query.OrderBy(u => u.Email);
                                break;
                            case "firstname":
                                query = filterCriteria.SortDescending ? 
                                    query.OrderByDescending(u => u.FirstName) : 
                                    query.OrderBy(u => u.FirstName);
                                break;
                            case "lastname":
                                query = filterCriteria.SortDescending ? 
                                    query.OrderByDescending(u => u.LastName) : 
                                    query.OrderBy(u => u.LastName);
                                break;
                            case "createdat":
                                query = filterCriteria.SortDescending ? 
                                    query.OrderByDescending(u => u.CreatedAt) : 
                                    query.OrderBy(u => u.CreatedAt);
                                break;
                            case "lastloginat":
                                query = filterCriteria.SortDescending ? 
                                    query.OrderByDescending(u => u.LastLoginAt) : 
                                    query.OrderBy(u => u.LastLoginAt);
                                break;
                            case "role":
                                query = filterCriteria.SortDescending ? 
                                    query.OrderByDescending(u => u.Role) : 
                                    query.OrderBy(u => u.Role);
                                break;
                            case "isactive":
                                query = filterCriteria.SortDescending ? 
                                    query.OrderByDescending(u => u.IsActive) : 
                                    query.OrderBy(u => u.IsActive);
                                break;
                            default: // Default sort by Id
                                query = filterCriteria.SortDescending ? 
                                    query.OrderByDescending(u => u.Id) : 
                                    query.OrderBy(u => u.Id);
                                break;
                        }
                    }
                    else
                    {
                        // Default sorting by Id if not specified
                        query = query.OrderBy(u => u.Id);
                    }
                }
                else
                {
                    // Default sorting by Id if no filter criteria
                    query = query.OrderBy(u => u.Id);
                }
                
                // Get the total count for the filtered query
                int totalCount = query.Count();
                
                // Apply pagination
                var pagedItems = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                
                // Create and return the paged result
                var result = new PagedResult<User>
                {
                    Items = pagedItems,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                
                return Task.FromResult(result);
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
