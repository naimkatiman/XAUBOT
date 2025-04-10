using System;

namespace XaubotClone.Utilities
{
    /// <summary>
    /// Provides helper methods for string manipulation.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Reverses the characters in a string.
        /// </summary>
        /// <param name="input">The string to reverse.</param>
        /// <returns>The reversed string, or null if the input is null.</returns>
        public static string? Reverse(string? input)
        {
            if (input == null)
            {
                return null;
            }
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// Checks if a string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <returns>True if the string is null, empty, or whitespace; otherwise, false.</returns>
        public static bool IsNullOrWhitespace(string? input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// Truncates a string to a specified maximum length, optionally adding an ellipsis.
        /// </summary>
        /// <param name="input">The string to truncate.</param>
        /// <param name="maxLength">The maximum length of the truncated string.</param>
        /// <param name="addEllipsis">Whether to add "..." if the string is truncated.</param>
        /// <returns>The truncated string.</returns>
        public static string Truncate(string? input, int maxLength, bool addEllipsis = true)
        {
            if (input == null || input.Length <= maxLength)
            {
                return input ?? string.Empty;
            }

            string truncated = input.Substring(0, maxLength);
            if (addEllipsis)
            {
                // Ensure ellipsis doesn't exceed maxLength
                if (maxLength >= 3)
                {
                     truncated = input.Substring(0, maxLength - 3) + "...";
                }
                else
                {
                    // Not enough space for ellipsis, just truncate
                    truncated = input.Substring(0, maxLength);
                }
            }
            return truncated;
        }
    }
} 