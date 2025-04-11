using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XaubotClone.Data
{
    public class MemoryDataRepository : IDataRepository
    {
        private readonly List<string> _items = new List<string> 
        { 
            "data1", "data2", "valueA", "valueB" 
        };
        private readonly object _lock = new object();

        public Task<IEnumerable<string>> GetAllAsync()
        {
            lock (_lock)
            {
                return Task.FromResult<IEnumerable<string>>(_items);
            }
        }

        public Task<string> GetByIdAsync(int id)
        {
            lock (_lock)
            {
                if (id < 0 || id >= _items.Count)
                    throw new ArgumentOutOfRangeException(nameof(id));

                return Task.FromResult(_items[id]);
            }
        }

        public Task<int> AddAsync(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                throw new ArgumentException("Item cannot be empty");

            lock (_lock)
            {
                _items.Add(item);
                return Task.FromResult(_items.Count - 1);
            }
        }

        public Task UpdateAsync(int id, string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                throw new ArgumentException("Item cannot be empty");

            lock (_lock)
            {
                if (id < 0 || id >= _items.Count)
                    throw new ArgumentOutOfRangeException(nameof(id));

                _items[id] = item;
                return Task.CompletedTask;
            }
        }

        public Task DeleteAsync(int id)
        {
            lock (_lock)
            {
                if (id < 0 || id >= _items.Count)
                    throw new ArgumentOutOfRangeException(nameof(id));

                _items.RemoveAt(id);
                return Task.CompletedTask;
            }
        }
    }
}
