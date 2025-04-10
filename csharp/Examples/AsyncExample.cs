using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace XaubotClone.Examples
{
    /// <summary>
    /// Demonstrates basic asynchronous operations, including fetching data from a URL.
    /// NOTE: This code is illustrative and requires a .NET runtime to execute.
    /// Running this directly in a Node.js environment will not work.
    /// Requires network connectivity to the target URL.
    /// </summary>
    public static class AsyncExample
    {
        // Use a static HttpClient instance for efficiency (see docs on HttpClient lifetime management)
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Simulates a long-running operation asynchronously.
        /// </summary>
        /// <param name="delayMilliseconds">Duration to simulate work.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task SimulateWorkAsync(int delayMilliseconds)
        {
            Console.WriteLine($"Starting simulated work for {delayMilliseconds}ms...");
            await Task.Delay(delayMilliseconds);
            Console.WriteLine("Simulated work finished.");
        }

        /// <summary>
        /// Fetches content from a given URL asynchronously.
        /// </summary>
        /// <param name="url">The URL to fetch content from.</param>
        /// <returns>A Task<string?> representing the asynchronous operation, containing the fetched content or null on error.</returns>
        public static async Task<string?> FetchUrlContentAsync(string url)
        {
            Console.WriteLine($"Attempting to fetch content from: {url}");
            try
            {
                // Ensure the URL is valid
                if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                    || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                {
                     Console.WriteLine("Invalid URL provided.");
                     return null;
                }

                // Send GET request asynchronously
                HttpResponseMessage response = await client.GetAsync(url);

                // Throw an exception if the request was not successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string asynchronously
                string content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Successfully fetched content (first 100 chars): {(content.Length > 100 ? content.Substring(0, 100) + "..." : content)}");
                return content;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return null;
            }
             catch (TaskCanceledException ex)
            {
                // Handle potential timeouts
                Console.WriteLine($"Request timed out or was canceled: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred during fetch: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Runs the asynchronous examples.
        /// Needs to be called from an async context (e.g., async Main method in a real .NET app).
        /// </summary>
        public static async Task RunExamplesAsync()
        {
            Console.WriteLine("--- Starting Async Examples ---");

            // Run simulated work
            await SimulateWorkAsync(1500);

            // Fetch content from a sample URL (using jsonplaceholder for testing)
            string? fetchedContent = await FetchUrlContentAsync("https://jsonplaceholder.typicode.com/todos/1");

            if (fetchedContent != null)
            {
                Console.WriteLine("Example fetch completed successfully.");
                // You could potentially parse the fetchedContent (which is JSON in this case)
                // using the JsonExample class if needed.
            }
            else
            {
                 Console.WriteLine("Example fetch failed.");
            }

             Console.WriteLine("--- Finished Async Examples ---");
        }
    }
} 