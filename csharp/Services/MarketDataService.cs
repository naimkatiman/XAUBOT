using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XaubotClone.Domain;
using XaubotClone.Data;

namespace XaubotClone.Services
{
    public interface IMarketDataService
    {
        Task<decimal> GetCurrentPriceAsync(TradingSymbol symbol);
        Task<Dictionary<TradingSymbol, decimal>> GetAllPricesAsync();
        Task<List<MarketDataPoint>> GetHistoricalDataAsync(TradingSymbol symbol, DateTime startDate, DateTime endDate, MarketDataInterval interval);
        Task<MarketSummary> GetMarketSummaryAsync(TradingSymbol symbol);
        Task<List<TradingSymbol>> GetAvailableSymbolsAsync();
        Task<SymbolInfo> GetSymbolInfoAsync(TradingSymbol symbol);
        Task<List<MarketAlert>> GetActiveAlertsAsync(int userId);
        Task<int> CreatePriceAlertAsync(int userId, TradingSymbol symbol, decimal targetPrice, PriceAlertType alertType);
        Task<bool> DeleteAlertAsync(int alertId);
        void SubscribeToUpdates(TradingSymbol symbol, Action<MarketDataPoint> callback);
        void UnsubscribeFromUpdates(TradingSymbol symbol, Action<MarketDataPoint> callback);
    }

    public class MarketDataService : IMarketDataService, IDisposable
    {
        private readonly IMarketDataRepository _marketDataRepository;
        private readonly INotificationService _notificationService;
        private readonly Dictionary<TradingSymbol, List<Action<MarketDataPoint>>> _subscribers = new Dictionary<TradingSymbol, List<Action<MarketDataPoint>>>();
        private readonly Dictionary<TradingSymbol, decimal> _cachedPrices = new Dictionary<TradingSymbol, decimal>();
        private readonly Timer _priceUpdateTimer;
        private readonly Timer _alertCheckTimer;
        private readonly object _lock = new object();
        private bool _disposed = false;

        public MarketDataService(IMarketDataRepository marketDataRepository, INotificationService notificationService)
        {
            _marketDataRepository = marketDataRepository ?? throw new ArgumentNullException(nameof(marketDataRepository));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            
            // Initialize market data updates timer - check every 5 seconds
            _priceUpdateTimer = new Timer(UpdatePrices, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            
            // Initialize alert check timer - check every 30 seconds
            _alertCheckTimer = new Timer(CheckAlerts, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
        }

        public async Task<decimal> GetCurrentPriceAsync(TradingSymbol symbol)
        {
            // Check the cache first
            lock (_lock)
            {
                if (_cachedPrices.TryGetValue(symbol, out decimal price))
                {
                    return price;
                }
            }

            // If not in cache, get from repository
            var latestPrice = await _marketDataRepository.GetLatestPriceAsync(symbol);
            
            // Update cache
            lock (_lock)
            {
                _cachedPrices[symbol] = latestPrice;
            }

            return latestPrice;
        }

        public async Task<Dictionary<TradingSymbol, decimal>> GetAllPricesAsync()
        {
            var prices = await _marketDataRepository.GetAllLatestPricesAsync();
            
            // Update cache
            lock (_lock)
            {
                foreach (var pair in prices)
                {
                    _cachedPrices[pair.Key] = pair.Value;
                }
            }

            return prices;
        }

        public async Task<List<MarketDataPoint>> GetHistoricalDataAsync(
            TradingSymbol symbol, 
            DateTime startDate, 
            DateTime endDate, 
            MarketDataInterval interval)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date must be before end date");

            return await _marketDataRepository.GetHistoricalDataAsync(symbol, startDate, endDate, interval);
        }

        public async Task<MarketSummary> GetMarketSummaryAsync(TradingSymbol symbol)
        {
            var last24Hours = DateTime.UtcNow.AddHours(-24);
            var data = await _marketDataRepository.GetHistoricalDataAsync(
                symbol, 
                last24Hours, 
                DateTime.UtcNow, 
                MarketDataInterval.Hour);

            if (data.Count == 0)
                throw new InvalidOperationException($"No data available for {symbol}");

            decimal currentPrice = data.Last().ClosePrice;
            decimal openPrice = data.First().OpenPrice;
            decimal highPrice = data.Max(d => d.HighPrice);
            decimal lowPrice = data.Min(d => d.LowPrice);
            decimal priceChange = currentPrice - openPrice;
            decimal percentChange = (priceChange / openPrice) * 100;
            decimal volume = data.Sum(d => d.Volume);

            return new MarketSummary
            {
                Symbol = symbol,
                CurrentPrice = currentPrice,
                OpenPrice = openPrice,
                HighPrice = highPrice,
                LowPrice = lowPrice,
                PriceChange = priceChange,
                PercentChange = percentChange,
                Volume = volume,
                Timestamp = DateTime.UtcNow
            };
        }

        public async Task<List<TradingSymbol>> GetAvailableSymbolsAsync()
        {
            return await _marketDataRepository.GetAvailableSymbolsAsync();
        }

        public async Task<SymbolInfo> GetSymbolInfoAsync(TradingSymbol symbol)
        {
            return await _marketDataRepository.GetSymbolInfoAsync(symbol);
        }

        public async Task<List<MarketAlert>> GetActiveAlertsAsync(int userId)
        {
            return await _marketDataRepository.GetActiveAlertsByUserIdAsync(userId);
        }

        public async Task<int> CreatePriceAlertAsync(int userId, TradingSymbol symbol, decimal targetPrice, PriceAlertType alertType)
        {
            var currentPrice = await GetCurrentPriceAsync(symbol);
            
            // Validate alert makes sense
            if (alertType == PriceAlertType.PriceAbove && currentPrice >= targetPrice)
                throw new ArgumentException($"Current price ({currentPrice}) is already above target price ({targetPrice})");
                
            if (alertType == PriceAlertType.PriceBelow && currentPrice <= targetPrice)
                throw new ArgumentException($"Current price ({currentPrice}) is already below target price ({targetPrice})");

            var alert = new MarketAlert
            {
                UserId = userId,
                Symbol = symbol,
                TargetPrice = targetPrice,
                AlertType = alertType,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            return await _marketDataRepository.AddAlertAsync(alert);
        }

        public async Task<bool> DeleteAlertAsync(int alertId)
        {
            await _marketDataRepository.DeleteAlertAsync(alertId);
            return true;
        }

        public void SubscribeToUpdates(TradingSymbol symbol, Action<MarketDataPoint> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            lock (_lock)
            {
                if (!_subscribers.ContainsKey(symbol))
                {
                    _subscribers[symbol] = new List<Action<MarketDataPoint>>();
                }

                _subscribers[symbol].Add(callback);
            }
        }

        public void UnsubscribeFromUpdates(TradingSymbol symbol, Action<MarketDataPoint> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            lock (_lock)
            {
                if (_subscribers.ContainsKey(symbol))
                {
                    _subscribers[symbol].Remove(callback);
                }
            }
        }

        private async void UpdatePrices(object state)
        {
            try
            {
                var latestData = await _marketDataRepository.GetAllLatestDataPointsAsync();
                
                lock (_lock)
                {
                    // Update cached prices
                    foreach (var dataPoint in latestData)
                    {
                        _cachedPrices[dataPoint.Symbol] = dataPoint.ClosePrice;
                        
                        // Notify subscribers
                        if (_subscribers.TryGetValue(dataPoint.Symbol, out var callbacks))
                        {
                            foreach (var callback in callbacks)
                            {
                                try
                                {
                                    callback(dataPoint);
                                }
                                catch
                                {
                                    // Ignore callback errors to prevent breaking the update loop
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Ignore errors to keep timer running
            }
        }

        private async void CheckAlerts(object state)
        {
            try
            {
                // Get all active alerts
                var activeAlerts = await _marketDataRepository.GetAllActiveAlertsAsync();
                
                // Get latest prices
                var prices = await GetAllPricesAsync();
                
                foreach (var alert in activeAlerts)
                {
                    if (!prices.TryGetValue(alert.Symbol, out decimal currentPrice))
                        continue;
                    
                    bool alertTriggered = false;
                    string message = string.Empty;
                    
                    // Check if alert conditions are met
                    if (alert.AlertType == PriceAlertType.PriceAbove && currentPrice >= alert.TargetPrice)
                    {
                        alertTriggered = true;
                        message = $"{alert.Symbol} price is now above your target of {alert.TargetPrice}. Current price: {currentPrice}";
                    }
                    else if (alert.AlertType == PriceAlertType.PriceBelow && currentPrice <= alert.TargetPrice)
                    {
                        alertTriggered = true;
                        message = $"{alert.Symbol} price is now below your target of {alert.TargetPrice}. Current price: {currentPrice}";
                    }
                    
                    if (alertTriggered)
                    {
                        // Mark alert as triggered
                        alert.IsActive = false;
                        alert.TriggeredAt = DateTime.UtcNow;
                        await _marketDataRepository.UpdateAlertAsync(alert);
                        
                        // Send notification to user
                        await _notificationService.SendPriceAlertAsync(alert.UserId, alert.Symbol, currentPrice, message);
                    }
                }
            }
            catch
            {
                // Ignore errors to keep timer running
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _priceUpdateTimer?.Dispose();
                    _alertCheckTimer?.Dispose();
                }

                _disposed = true;
            }
        }

        ~MarketDataService()
        {
            Dispose(false);
        }
    }
}
