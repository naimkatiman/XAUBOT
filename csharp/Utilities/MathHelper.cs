using System;

namespace XaubotClone.Utilities
{
    /// <summary>
    /// Provides helper methods for mathematical operations.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Calculates the factorial of a non-negative integer.
        /// </summary>
        /// <param name="n">The non-negative integer.</param>
        /// <returns>The factorial of n.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if n is negative.</exception>
        public static long Factorial(int n)
        {
            if (n < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Factorial is not defined for negative numbers.");
            }
            if (n == 0)
            {
                return 1;
            }
            long result = 1;
            for (int i = 1; i <= n; i++)
            {
                // Check for potential overflow before multiplying
                if (long.MaxValue / i < result)
                {
                    throw new OverflowException("Factorial calculation resulted in an overflow.");
                }
                result *= i;
            }
            return result;
        }

        /// <summary>
        /// Checks if a number is prime.
        /// </summary>
        /// <param name="number">The number to check.</param>
        /// <returns>True if the number is prime; otherwise, false.</returns>
        public static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }

         /// <summary>
        /// Clamps a value between a minimum and maximum value.
        /// </summary>
        /// <typeparam name="T">The type of the value, must implement IComparable.</typeparam>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>The clamped value.</returns>
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }
    }
} 