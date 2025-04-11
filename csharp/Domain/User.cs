using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XaubotClone.Domain
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }

        public bool IsActive { get; set; } = true;

        public UserRole Role { get; set; } = UserRole.Regular;

        public List<UserPreference> Preferences { get; set; } = new List<UserPreference>();
        
        public List<TradingActivity> TradingActivities { get; set; } = new List<TradingActivity>();

        public string FullName => $"{FirstName} {LastName}".Trim();

        public bool IsAdmin => Role == UserRole.Admin;

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void PromoteToAdmin()
        {
            Role = UserRole.Admin;
        }

        public void DemoteToRegular()
        {
            Role = UserRole.Regular;
        }

        public bool HasPreference(string key)
        {
            return Preferences.Exists(p => p.Key == key);
        }

        public string GetPreference(string key, string defaultValue = null)
        {
            var preference = Preferences.Find(p => p.Key == key);
            return preference?.Value ?? defaultValue;
        }

        public void SetPreference(string key, string value)
        {
            var preference = Preferences.Find(p => p.Key == key);
            if (preference != null)
            {
                preference.Value = value;
                preference.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                Preferences.Add(new UserPreference
                {
                    UserId = Id,
                    Key = key,
                    Value = value,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }
    }
}
