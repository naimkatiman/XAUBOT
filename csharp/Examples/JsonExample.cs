using System;
using System.Text.Json;
using System.Collections.Generic;

namespace XaubotClone.Examples
{
    /// <summary>
    /// Represents a simple data structure for JSON examples.
    /// </summary>
    public class UserData
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// Demonstrates JSON serialization and deserialization using System.Text.Json.
    /// NOTE: This code is illustrative and requires a .NET runtime to execute.
    /// </summary>
    public static class JsonExample
    {
        /// <summary>
        /// Serializes a UserData object to a JSON string.
        /// </summary>
        /// <param name="user">The UserData object to serialize.</param>
        /// <returns>A JSON string representation of the object, or null on error.</returns>
        public static string? SerializeUserToJson(UserData user)
        {
            try
            {
                // Configure serialization options (e.g., indentation)
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true, // Makes the output readable
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Use camelCase for property names in JSON
                };

                string jsonString = JsonSerializer.Serialize(user, options);
                Console.WriteLine("--- Serialized JSON ---");
                Console.WriteLine(jsonString);
                Console.WriteLine("-----------------------");
                return jsonString;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error during JSON serialization: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Deserializes a JSON string back into a UserData object.
        /// </summary>
        /// <param name="jsonString">The JSON string to deserialize.</param>
        /// <returns>A UserData object, or null if deserialization fails.</returns>
        public static UserData? DeserializeJsonToUser(string jsonString)
        {
             try
            {
                 // Configure deserialization options (must match serialization if custom policies were used)
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Allows matching properties regardless of case
                };

                UserData? user = JsonSerializer.Deserialize<UserData>(jsonString, options);

                if (user != null)
                {
                     Console.WriteLine("--- Deserialized UserData ---");
                     Console.WriteLine($"UserId: {user.UserId}");
                     Console.WriteLine($"Username: {user.Username}");
                     Console.WriteLine($"Email: {user.Email}");
                     Console.WriteLine($"IsActive: {user.IsActive}");
                     Console.WriteLine($"Roles: {string.Join(", ", user.Roles)}");
                     Console.WriteLine($"CreatedDate: {user.CreatedDate}");
                     Console.WriteLine("---------------------------");
                }
                else
                {
                    Console.WriteLine("Deserialization resulted in a null object.");
                }

                return user;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error during JSON deserialization: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Runs a simple serialization/deserialization example.
        /// </summary>
        public static void RunExample()
        {
            var user = new UserData
            {
                UserId = 101,
                Username = "johndoe",
                Email = "john.doe@example.com",
                IsActive = true,
                Roles = new List<string> { "Admin", "User" },
                CreatedDate = DateTime.UtcNow
            };

            string? json = SerializeUserToJson(user);

            if (!string.IsNullOrEmpty(json))
            {
                DeserializeJsonToUser(json);
            }
        }
    }
} 