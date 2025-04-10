using System;
using System.Collections.Generic;

namespace XaubotClone.Api
{
    // Placeholder for a potential API controller.
    // In a real ASP.NET Core application, this would likely inherit from ControllerBase
    // and use attributes like [ApiController], [Route], [HttpGet], [HttpPost], etc.

    public class ExampleController
    {
        // Example placeholder method representing an API endpoint to get data.
        public List<string> GetData()
        {
            // In a real API, this would fetch data from a database or another service.
            return new List<string> { "data1", "data2", "valueA", "valueB" };
        }

        // Example placeholder method representing an API endpoint to add data.
        // It might take some input model as a parameter.
        public bool AddData(string newData)
        {
            if (string.IsNullOrWhiteSpace(newData))
            {
                return false; // Indicate failure
            }

            // In a real API, this would save the data to a database or other storage.
            Console.WriteLine($"Placeholder: Received data to add: {newData}");

            // Simulate success
            return true;
        }

        // Example placeholder method with a parameter.
        public string GetDataById(int id)
        {
            // Simulate fetching data based on an ID.
            if (id < 0 || id >= GetData().Count)
            {
                // In a real API, might return NotFound() or similar.
                return "Error: Invalid ID";
            }
            return $"Data for ID {id}: {GetData()[id]}";
        }
    }
} 