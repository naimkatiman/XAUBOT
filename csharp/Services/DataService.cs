using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XaubotClone.Data;

namespace XaubotClone.Services
{
    public interface IDataService
    {
        Task<IEnumerable<string>> GetAllItemsAsync();
        Task<string> GetItemByIdAsync(int id);
        Task<int> AddItemAsync(string item);
        Task UpdateItemAsync(int id, string item);
        Task DeleteItemAsync(int id);
    }

    public class DataService : IDataService
    {
        private readonly IDataRepository _repository;

        public DataService(IDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<string>> GetAllItemsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<string> GetItemByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> AddItemAsync(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                throw new ArgumentException("Item cannot be empty");

            return await _repository.AddAsync(item);
        }

        public async Task UpdateItemAsync(int id, string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                throw new ArgumentException("Item cannot be empty");

            await _repository.UpdateAsync(id, item);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
