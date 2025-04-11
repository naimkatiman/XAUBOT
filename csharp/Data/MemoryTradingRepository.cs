using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XaubotClone.Domain;

namespace XaubotClone.Data
{
    public class MemoryTradingRepository : ITradingRepository
    {
        private readonly List<TradingActivity> _tradingActivities = new List<TradingActivity>
        {
            new TradingActivity
            {
                Id = 1,
                UserId = 2,
                Symbol = TradingSymbol.XAUUSD,
                Amount = 1.0m,
                EntryPrice = 1950.50m,
                Position = TradingPosition.Long,
                Status = TradingStatus.Closed,
                OpenTime = DateTime.UtcNow.AddDays(-10),
                CloseTime = DateTime.UtcNow.AddDays(-9),
                ExitPrice = 1980.25m,
                StopLoss = 1930.0m,
                TakeProfit = 2000.0m,
                Notes = "Gold breaking resistance level"
            },
            new TradingActivity
            {
                Id = 2,
                UserId = 2,
                Symbol = TradingSymbol.EURUSD,
                Amount = 10000.0m,
                EntryPrice = 1.08m,
                Position = TradingPosition.Short,
                Status = TradingStatus.Closed,
                OpenTime = DateTime.UtcNow.AddDays(-8),
                CloseTime = DateTime.UtcNow.AddDays(-7),
                ExitPrice = 1.075m,
                StopLoss = 1.09m,
                TakeProfit = 1.07m,
                Notes = "EUR/USD bearish trend"
            },
            new TradingActivity
            {
                Id = 3,
                UserId = 3,
                Symbol = TradingSymbol.XAUUSD,
                Amount = 2.0m,
                EntryPrice = 1980.0m,
                Position = TradingPosition.Long,
                Status = TradingStatus.Open,
                OpenTime = DateTime.UtcNow.AddDays(-2),
                StopLoss = 1960.0m,
                TakeProfit = 2020.0m,
                Notes = "Gold bullish momentum"
            },
            new TradingActivity
            {
                Id = 4,
                UserId = 3,
                Symbol = TradingSymbol.BTCUSD,
                Amount = 0.5m,
                EntryPrice = 65000.0m,
                Position = TradingPosition.Short,
                Status = TradingStatus.Open,
                OpenTime = DateTime.UtcNow.AddDays(-1),
                StopLoss = 67000.0m,
                TakeProfit = 60000.0m,
                Notes = "BTC correction expected"
            }
        };

        private readonly object _lock = new object();
        private int _nextId = 5;

        public Task<List<TradingActivity>> GetAllAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_tradingActivities.ToList());
            }
        }

        public Task<TradingActivity> GetByIdAsync(int id)
        {
            lock (_lock)
            {
                var activity = _tradingActivities.FirstOrDefault(a => a.Id == id);
                return Task.FromResult(activity);
            }
        }

        public Task<List<TradingActivity>> GetByUserIdAsync(int userId)
        {
            lock (_lock)
            {
                var activities = _tradingActivities
                    .Where(a => a.UserId == userId)
                    .ToList();
                    
                return Task.FromResult(activities);
            }
        }

        public Task<List<TradingActivity>> GetByStatusAsync(TradingStatus status)
        {
            lock (_lock)
            {
                var activities = _tradingActivities
                    .Where(a => a.Status == status)
                    .ToList();
                    
                return Task.FromResult(activities);
            }
        }

        public Task<List<TradingActivity>> GetBySymbolAsync(TradingSymbol symbol)
        {
            lock (_lock)
            {
                var activities = _tradingActivities
                    .Where(a => a.Symbol == symbol)
                    .ToList();
                    
                return Task.FromResult(activities);
            }
        }

        public Task<int> AddAsync(TradingActivity tradingActivity)
        {
            if (tradingActivity == null)
                throw new ArgumentNullException(nameof(tradingActivity));
                
            lock (_lock)
            {
                // Assign ID
                tradingActivity.Id = _nextId++;
                
                // Add to collection
                _tradingActivities.Add(tradingActivity);
                
                return Task.FromResult(tradingActivity.Id);
            }
        }

        public Task UpdateAsync(TradingActivity tradingActivity)
        {
            if (tradingActivity == null)
                throw new ArgumentNullException(nameof(tradingActivity));
                
            lock (_lock)
            {
                // Find activity in collection
                var index = _tradingActivities.FindIndex(a => a.Id == tradingActivity.Id);
                
                if (index < 0)
                    throw new ArgumentException($"Trading activity with ID {tradingActivity.Id} not found");
                    
                // Update activity in collection
                _tradingActivities[index] = tradingActivity;
                
                return Task.CompletedTask;
            }
        }

        public Task DeleteAsync(int id)
        {
            lock (_lock)
            {
                // Find activity in collection
                var index = _tradingActivities.FindIndex(a => a.Id == id);
                
                if (index < 0)
                    throw new ArgumentException($"Trading activity with ID {id} not found");
                    
                // Remove from collection
                _tradingActivities.RemoveAt(index);
                
                return Task.CompletedTask;
            }
        }
    }
}
