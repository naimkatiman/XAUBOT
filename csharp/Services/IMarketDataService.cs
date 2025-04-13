using System;
using System.Threading.Tasks;
using XaubotClone.Domain;

namespace XaubotClone.Services
{
    public interface IMarketDataService
    {
        Task<decimal> GetCurrentPriceAsync(TradingSymbol symbol);
        Task<decimal> GetDailyHighAsync(TradingSymbol symbol);
        Task<decimal> GetDailyLowAsync(TradingSymbol symbol);
        Task<decimal> GetOpenPriceAsync(TradingSymbol symbol);
        Task<decimal> GetPreviousClosePriceAsync(TradingSymbol symbol);
        Task<decimal> GetVolumeAsync(TradingSymbol symbol);
        Task<decimal> GetPriceAtTimeAsync(TradingSymbol symbol, DateTime time);
        Task SubscribeToMarketUpdatesAsync(TradingSymbol symbol, Action<decimal> priceUpdateCallback);
        Task UnsubscribeFromMarketUpdatesAsync(TradingSymbol symbol, Action<decimal> priceUpdateCallback);
    }
}
