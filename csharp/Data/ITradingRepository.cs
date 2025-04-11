using System.Collections.Generic;
using System.Threading.Tasks;
using XaubotClone.Domain;

namespace XaubotClone.Data
{
    public interface ITradingRepository
    {
        Task<List<TradingActivity>> GetAllAsync();
        Task<TradingActivity> GetByIdAsync(int id);
        Task<List<TradingActivity>> GetByUserIdAsync(int userId);
        Task<List<TradingActivity>> GetByStatusAsync(TradingStatus status);
        Task<List<TradingActivity>> GetBySymbolAsync(TradingSymbol symbol);
        Task<int> AddAsync(TradingActivity tradingActivity);
        Task UpdateAsync(TradingActivity tradingActivity);
        Task DeleteAsync(int id);
    }
}
