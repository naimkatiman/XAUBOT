using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using XaubotClone.Domain;

namespace XaubotClone.Services
{
    public class MarketDataService : IMarketDataService
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<TradingSymbol, List<Action<decimal>>> _subscribers;
        
        // Mock data for demonstration purposes
        private static readonly Dictionary<TradingSymbol, MarketData> _mockMarketData = new Dictionary<TradingSymbol, MarketData>
        {
            { TradingSymbol.XAUUSD, new MarketData { CurrentPrice = 2024.55m, DailyHigh = 2030.25m, DailyLow = 2018.75m, OpenPrice = 2022.15m, PreviousClose = 2021.90m, Volume = 15420 } },
            { TradingSymbol.XAGUSD, new MarketData { CurrentPrice = 25.67m, DailyHigh = 25.89m, DailyLow = 25.42m, OpenPrice = 25.65m, PreviousClose = 25.58m, Volume = 32150 } },
            { TradingSymbol.EURUSD, new MarketData { CurrentPrice = 1.0815m, DailyHigh = 1.0845m, DailyLow = 1.0792m, OpenPrice = 1.0825m, PreviousClose = 1.0830m, Volume = 87650 } },
            { TradingSymbol.GBPUSD, new MarketData { CurrentPrice = 1.2645m, DailyHigh = 1.2675m, DailyLow = 1.2610m, OpenPrice = 1.2635m, PreviousClose = 1.2640m, Volume = 65430 } },
            { TradingSymbol.USDJPY, new MarketData { CurrentPrice = 155.65m, DailyHigh = 156.10m, DailyLow = 155.25m, OpenPrice = 155.40m, PreviousClose = 155.50m, Volume = 74320 } },
            { TradingSymbol.BTCUSD, new MarketData { CurrentPrice = 68452.33m, DailyHigh = 69125.75m, DailyLow = 67895.42m, OpenPrice = 68225.50m, PreviousClose = 68125.25m, Volume = 12540 } },
            { TradingSymbol.ETHUSD, new MarketData { CurrentPrice = 3224.45m, DailyHigh = 3275.20m, DailyLow = 3198.65m, OpenPrice = 3210.75m, PreviousClose = 3215.50m, Volume = 18650 } }
        };
        
        // Historical price simulation for demonstration
        private static readonly Random _random = new Random();
        
        public MarketDataService(HttpClient httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _subscribers = new Dictionary<TradingSymbol, List<Action<decimal>>>();
            
            foreach (TradingSymbol symbol in Enum.GetValues(typeof(TradingSymbol)))
            {
                _subscribers[symbol] = new List<Action<decimal>>();
            }
            
            // Start a background task to simulate price updates
            Task.Run(SimulatePriceUpdatesAsync);
        }
        
        public Task<decimal> GetCurrentPriceAsync(TradingSymbol symbol)
        {
            if (_mockMarketData.TryGetValue(symbol, out var data))
            {
                return Task.FromResult(data.CurrentPrice);
            }
            
            throw new ArgumentException($"Market data for symbol {symbol} not available");
        }
        
        public Task<decimal> GetDailyHighAsync(TradingSymbol symbol)
        {
            if (_mockMarketData.TryGetValue(symbol, out var data))
            {
                return Task.FromResult(data.DailyHigh);
            }
            
            throw new ArgumentException($"Market data for symbol {symbol} not available");
        }
        
        public Task<decimal> GetDailyLowAsync(TradingSymbol symbol)
        {
            if (_mockMarketData.TryGetValue(symbol, out var data))
            {
                return Task.FromResult(data.DailyLow);
            }
            
            throw new ArgumentException($"Market data for symbol {symbol} not available");
        }
        
        public Task<decimal> GetOpenPriceAsync(TradingSymbol symbol)
        {
            if (_mockMarketData.TryGetValue(symbol, out var data))
            {
                return Task.FromResult(data.OpenPrice);
            }
            
            throw new ArgumentException($"Market data for symbol {symbol} not available");
        }
        
        public Task<decimal> GetPreviousClosePriceAsync(TradingSymbol symbol)
        {
            if (_mockMarketData.TryGetValue(symbol, out var data))
            {
                return Task.FromResult(data.PreviousClose);
            }
            
            throw new ArgumentException($"Market data for symbol {symbol} not available");
        }
        
        public Task<decimal> GetVolumeAsync(TradingSymbol symbol)
        {
            if (_mockMarketData.TryGetValue(symbol, out var data))
            {
                return Task.FromResult(data.Volume);
            }
            
            throw new ArgumentException($"Market data for symbol {symbol} not available");
        }
        
        public Task<decimal> GetPriceAtTimeAsync(TradingSymbol symbol, DateTime time)
        {
            // This is a mock implementation that generates a realistic-looking historical price
            // based on the current price and how far in the past the requested time is
            
            if (_mockMarketData.TryGetValue(symbol, out var data))
            {
                var timeDiff = DateTime.UtcNow - time;
                var daysInPast = timeDiff.TotalDays;
                
                if (daysInPast < 0)
                {
                    throw new ArgumentException("Cannot fetch future prices");
                }
                
                // Small randomized variation from current price based on how far in the past we're looking
                var volatilityFactor = GetVolatilityFactor(symbol);
                var variation = (decimal)(Math.Sqrt(daysInPast) * volatilityFactor * (_random.NextDouble() - 0.5));
                
                // The further in the past, the more potential variation
                return Task.FromResult(data.CurrentPrice * (1 + variation));
            }
            
            throw new ArgumentException($"Market data for symbol {symbol} not available");
        }
        
        public Task SubscribeToMarketUpdatesAsync(TradingSymbol symbol, Action<decimal> priceUpdateCallback)
        {
            if (!_subscribers.ContainsKey(symbol))
            {
                _subscribers[symbol] = new List<Action<decimal>>();
            }
            
            _subscribers[symbol].Add(priceUpdateCallback);
            return Task.CompletedTask;
        }
        
        public Task UnsubscribeFromMarketUpdatesAsync(TradingSymbol symbol, Action<decimal> priceUpdateCallback)
        {
            if (_subscribers.ContainsKey(symbol))
            {
                _subscribers[symbol].Remove(priceUpdateCallback);
            }
            
            return Task.CompletedTask;
        }
        
        private async Task SimulatePriceUpdatesAsync()
        {
            while (true)
            {
                foreach (var symbol in _mockMarketData.Keys.ToList())
                {
                    // Generate small price movement
                    var volatilityFactor = GetVolatilityFactor(symbol);
                    var priceMove = (decimal)(_random.NextDouble() * volatilityFactor - volatilityFactor / 2);
                    
                    var data = _mockMarketData[symbol];
                    var newPrice = data.CurrentPrice * (1 + priceMove);
                    
                    // Update current price
                    data.CurrentPrice = newPrice;
                    
                    // Update high/low if needed
                    if (newPrice > data.DailyHigh)
                        data.DailyHigh = newPrice;
                    if (newPrice < data.DailyLow)
                        data.DailyLow = newPrice;
                        
                    // Update volume
                    data.Volume += _random.Next(50, 200);
                    
                    // Notify subscribers
                    if (_subscribers.TryGetValue(symbol, out var callbacks))
                    {
                        foreach (var callback in callbacks.ToList())
                        {
                            callback(newPrice);
                        }
                    }
                }
                
                // Wait a short interval before the next update
                await Task.Delay(2000); // 2 seconds between updates
            }
        }
        
        private double GetVolatilityFactor(TradingSymbol symbol)
        {
            // Different assets have different volatility
            return symbol switch
            {
                TradingSymbol.XAUUSD => 0.001, // 0.1% 
                TradingSymbol.XAGUSD => 0.002, // 0.2%
                TradingSymbol.EURUSD => 0.0005, // 0.05%
                TradingSymbol.GBPUSD => 0.0007, // 0.07%
                TradingSymbol.USDJPY => 0.0006, // 0.06%
                TradingSymbol.BTCUSD => 0.01, // 1%
                TradingSymbol.ETHUSD => 0.015, // 1.5%
                _ => 0.001
            };
        }
    }
    
    internal class MarketData
    {
        public decimal CurrentPrice { get; set; }
        public decimal DailyHigh { get; set; }
        public decimal DailyLow { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal PreviousClose { get; set; }
        public decimal Volume { get; set; }
    }
}
