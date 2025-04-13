using System.Collections.Generic;
using System.Threading.Tasks;
using XaubotClone.Domain;

namespace XaubotClone.Services
{
    public interface ITradingService
    {
        Task<List<TradingActivity>> GetAllTradingActivitiesAsync();
        Task<TradingActivity> GetTradingActivityByIdAsync(int id);
        Task<List<TradingActivity>> GetTradingActivitiesByUserIdAsync(int userId);
        Task<List<TradingActivity>> GetTradingActivitiesByStatusAsync(TradingStatus status);
        Task<List<TradingActivity>> GetTradingActivitiesBySymbolAsync(TradingSymbol symbol);
        Task<int> OpenTradingPositionAsync(int userId, TradingSymbol symbol, decimal amount, decimal entryPrice, TradingPosition position, decimal? stopLoss = null, decimal? takeProfit = null, string notes = null);
        Task CloseTradingPositionAsync(int tradingActivityId, decimal exitPrice);
        Task CancelTradingPositionAsync(int tradingActivityId);
        Task UpdateStopLossAsync(int tradingActivityId, decimal? stopLoss);
        Task UpdateTakeProfitAsync(int tradingActivityId, decimal? takeProfit);
        Task<decimal> CalculateTotalProfitLossAsync(int userId);
        Task<Dictionary<TradingSymbol, decimal>> GetProfitLossBySymbolAsync(int userId);
    }
}
