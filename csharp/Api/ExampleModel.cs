using System;

namespace XaubotClone.Api
{
    /// <summary>
    /// Placeholder for a data model that might be used by the API.
    /// In a real application, this could represent data from a database table
    /// or data transferred between the client and server.
    /// </summary>
    public class ExampleModel
    {
        /// <summary>
        /// Gets or sets an identifier for the model.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a name property.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets a timestamp indicating when the data was created or last modified.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the item is active.
        /// </summary>
        public bool IsActive { get; set; }

        public ExampleModel()
        {
            // Initialize default values if necessary
            Timestamp = DateTime.UtcNow;
            IsActive = true;
        }
    }
} 