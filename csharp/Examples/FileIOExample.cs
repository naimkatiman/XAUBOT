using System;
using System.IO;
using System.Text;

namespace XaubotClone.Examples
{
    /// <summary>
    /// Demonstrates basic file input/output operations using System.IO.
    /// NOTE: This code is illustrative and requires a .NET runtime to execute.
    /// Running this directly in a Node.js environment will not work.
    /// File paths used here are examples and may need adjustment based on the execution context.
    /// </summary>
    public static class FileIOExample
    {
        private static string exampleFilePath = "example_output.txt"; // Example file path

        /// <summary>
        /// Writes text content to a file. Overwrites the file if it exists.
        /// </summary>
        /// <param name="content">The text content to write.</param>
        public static void WriteTextToFile(string content)
        {
            try
            {
                // Using StreamWriter for efficient text writing
                // The 'using' statement ensures the stream is properly disposed
                using (StreamWriter writer = new StreamWriter(exampleFilePath, false, Encoding.UTF8)) // false = overwrite
                {
                    writer.Write(content);
                }
                Console.WriteLine($"Successfully wrote content to '{exampleFilePath}'");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An IO error occurred while writing: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Appends text content to a file. Creates the file if it doesn't exist.
        /// </summary>
        /// <param name="content">The text content to append.</param>
        public static void AppendTextToFile(string content)
        {
             try
            {
                using (StreamWriter writer = new StreamWriter(exampleFilePath, true, Encoding.UTF8)) // true = append
                {
                    writer.WriteLine(content); // Using WriteLine to add a newline
                }
                Console.WriteLine($"Successfully appended content to '{exampleFilePath}'");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An IO error occurred while appending: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Reads all text content from a file.
        /// </summary>
        /// <returns>The content read from the file, or null if an error occurs or the file doesn't exist.</returns>
        public static string? ReadTextFromFile()
        {
             try
            {
                if (!File.Exists(exampleFilePath))
                {
                    Console.WriteLine($"File not found: '{exampleFilePath}'");
                    return null;
                }

                // Using StreamReader for efficient text reading
                using (StreamReader reader = new StreamReader(exampleFilePath, Encoding.UTF8))
                {
                    string content = reader.ReadToEnd();
                    Console.WriteLine($"Successfully read content from '{exampleFilePath}'");
                    return content;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An IO error occurred while reading: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Deletes the example file.
        /// </summary>
        public static void DeleteExampleFile()
        {
            try
            {
                if (File.Exists(exampleFilePath))
                {
                    File.Delete(exampleFilePath);
                    Console.WriteLine($"Successfully deleted file: '{exampleFilePath}'");
                }
                else
                {
                     Console.WriteLine($"File not found, nothing to delete: '{exampleFilePath}'");
                }
            }
             catch (IOException ex)
            {
                Console.WriteLine($"An IO error occurred while deleting: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
} 