using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XaubotClone.Data
{
    public interface IDataRepository
    {
        Task<IEnumerable<string>> GetAllAsync();
        Task<string> GetByIdAsync(int id);
        Task<int> AddAsync(string item);
        Task UpdateAsync(int id, string item);
        Task DeleteAsync(int id);
    }
}
