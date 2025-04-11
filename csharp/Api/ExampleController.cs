using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XaubotClone.Services;

namespace XaubotClone.Api
{
    /// <summary>
    /// Example API controller demonstrating various endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly IDataService _dataService;

        public ExampleController(IDataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Gets all available data items
        /// </summary>
        /// <returns>List of data items</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllData()
        {
            var items = await _dataService.GetAllItemsAsync();
            return Ok(items);
        }

        /// <summary>
        /// Gets a specific data item by ID
        /// </summary>
        /// <param name="id">The ID of the item to retrieve</param>
        /// <returns>The requested data item</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetDataById(int id)
        {
            try
            {
                var item = await _dataService.GetItemByIdAsync(id);
                return Ok(item);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound($"Item with ID {id} not found");
            }
        }

        /// <summary>
        /// Adds a new data item
        /// </summary>
        /// <param name="newData">The data to add</param>
        /// <returns>Result of the operation</returns>
        [HttpPost]
        public async Task<IActionResult> AddData([FromBody] string newData)
        {
            try
            {
                var id = await _dataService.AddItemAsync(newData);
                return CreatedAtAction(nameof(GetDataById), new { id }, newData);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing data item
        /// </summary>
        /// <param name="id">ID of item to update</param>
        /// <param name="updatedData">New data value</param>
        /// <returns>Result of the operation</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateData(int id, [FromBody] string updatedData)
        {
            try
            {
                await _dataService.UpdateItemAsync(id, updatedData);
                return NoContent();
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound($"Item with ID {id} not found");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a data item
        /// </summary>
        /// <param name="id">ID of item to delete</param>
        /// <returns>Result of the operation</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            try
            {
                await _dataService.DeleteItemAsync(id);
                return NoContent();
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound($"Item with ID {id} not found");
            }
        }
    }
}