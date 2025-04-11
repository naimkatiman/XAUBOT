using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XaubotClone.Data;
using XaubotClone.Domain;

namespace XaubotClone.Services
{
    public interface ITradingService
    {
        Task<List<TradingActivity>> GetUserTradingActivitiesAsync(int userId);
        Task<TradingActivity> GetTradingActivityByIdAsync(int id);
        Task<int> OpenPositionAsync(int userId, TradingSymbol symbol, TradingPosition position, decimal amount, decimal entryPrice, decimal? stopLoss = null, decimal? takeProfit = null, string notes = null);
        Task<TradingActivity> ClosePositionAsync(int tradingActivityId, decimal exitPrice);
        Task<TradingActivity> CancelPositionAsync(int tradingActivityId);
        Task<bool> UpdateStopLossAsync(int tradingActivityId, decimal stopLoss);
        Task<bool> UpdateTakeProfitAsync(int tradingActivityId, decimal takeProfit);
        Task<List<TradingActivity>> GetOpenPositionsAsync();
        Task<decimal> CalculateTotalProfitLossAsync(int userId);
        Task<decimal> GetCurrentExposureAsync(int userId, TradingSymbol? symbol = null);
    }
    
    public class TradingService : ITradingService
    {
        private readonly ITradingRepository _tradingRepository;
        private readonly IUserRepository _userRepository;
        
        public TradingService(ITradingRepository tradingRepository, IUserRepository userRepository)
        {
            _tradingRepository = tradingRepository;
            _userRepository = userRepository;
        }
        
        public async Task<List<TradingActivity>> GetUserTradingActivitiesAsync(int userId)
        {
            // Validate user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");
                
            return await _tradingRepository.GetByUserIdAsync(userId);
        }
        
        public async Task<TradingActivity> GetTradingActivityByIdAsync(int id)
        {
            return await _tradingRepository.GetByIdAsync(id);
        }
        
        public async Task<int> OpenPositionAsync(int userId, TradingSymbol symbol, TradingPosition position, decimal amount, decimal entryPrice, decimal? stopLoss = null, decimal? takeProfit = null, string notes = null)
        {
            // Validate user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");
                
            // Validate input
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero");
                
            if (entryPrice <= 0)
                throw new ArgumentException("Entry price must be greater than zero");
                
            // Validate stop loss and take profit based on position type
            if (stopLoss.HasValue && position == TradingPosition.Long && stopLoss.Value >= entryPrice)
                throw new ArgumentException("For long positions, stop loss must be lower than entry price");
                
            if (stopLoss.HasValue && position == TradingPosition.Short && stopLoss.Value <= entryPrice)
                throw new ArgumentException("For short positions, stop loss must be higher than entry price");
                
            if (takeProfit.HasValue && position == TradingPosition.Long && takeProfit.Value <= entryPrice)
                throw new ArgumentException("For long positions, take profit must be higher than entry price");
                
            if (takeProfit.HasValue && position == TradingPosition.Short && takeProfit.Value >= entryPrice)
                throw new ArgumentException("For short positions, take profit must be lower than entry price");
                
            // Create trading activity
            var tradingActivity = new TradingActivity
            {
                UserId = userId,
                Symbol = symbol,
                Position = position,
                Amount = amount,
                EntryPrice = entryPrice,
                StopLoss = stopLoss,
                TakeProfit = takeProfit,
                Notes = notes,
                Status = TradingStatus.Open,
                OpenTime = DateTime.UtcNow
            };
            
            // Add to repository
            return await _tradingRepository.AddAsync(tradingActivity);
        }
        
        public async Task<TradingActivity> ClosePositionAsync(int tradingActivityId, decimal exitPrice)
        {
            if (exitPrice <= 0)
                throw new ArgumentException("Exit price must be greater than zero");
                
            var tradingActivity = await _tradingRepository.GetByIdAsync(tradingActivityId);
            if (tradingActivity == null)
                throw new ArgumentException($"Trading activity with ID {tradingActivityId} not found");
                
            if (tradingActivity.Status != TradingStatus.Open)
                throw new InvalidOperationException($"Trading activity with ID {tradingActivityId} is not open");
                
            // Close position
            tradingActivity.Close(exitPrice);
            
            // Update in repository
            await _tradingRepository.UpdateAsync(tradingActivity);
            
            return tradingActivity;
        }
        
        public async Task<TradingActivity> CancelPositionAsync(int tradingActivityId)
        {
            var tradingActivity = await _tradingRepository.GetByIdAsync(tradingActivityId);
            if (tradingActivity == null)
                throw new ArgumentException($"Trading activity with ID {tradingActivityId} not found");
                
            if (tradingActivity.Status != TradingStatus.Open && tradingActivity.Status != TradingStatus.Pending)
                throw new InvalidOperationException($"Trading activity with ID {tradingActivityId} cannot be cancelled");
                
            // Cancel position
            tradingActivity.Cancel();
            
            // Update in repository
            await _tradingRepository.UpdateAsync(tradingActivity);
            
            return tradingActivity;
        }
        
        public async Task<bool> UpdateStopLossAsync(int tradingActivityId, decimal stopLoss)
        {
            if (stopLoss <= 0)
                throw new ArgumentException("Stop loss must be greater than zero");
                
            var tradingActivity = await _tradingRepository.GetByIdAsync(tradingActivityId);
            if (tradingActivity == null)
                throw new ArgumentException($"Trading activity with ID {tradingActivityId} not found");
                
            if (tradingActivity.Status != TradingStatus.Open)
                throw new InvalidOperationException($"Trading activity with ID {tradingActivityId} is not open");
                
            // Validate stop loss based on position type
            if (tradingActivity.Position == TradingPosition.Long && stopLoss >= tradingActivity.EntryPrice)
                throw new ArgumentException("For long positions, stop loss must be lower than entry price");
                
            if (tradingActivity.Position == TradingPosition.Short && stopLoss <= tradingActivity.EntryPrice)
                throw new ArgumentException("For short positions, stop loss must be higher than entry price");
                
            // Update stop loss
            tradingActivity.StopLoss = stopLoss;
            
            // Update in repository
            await _tradingRepository.UpdateAsync(tradingActivity);
            
            return true;
        }
        
        public async Task<bool> UpdateTakeProfitAsync(int tradingActivityId, decimal takeProfit)
        {
            if (takeProfit <= 0)
                throw new ArgumentException("Take profit must be greater than zero");
                
            var tradingActivity = await _tradingRepository.GetByIdAsync(tradingActivityId);
            if (tradingActivity == null)
                throw new ArgumentException($"Trading activity with ID {tradingActivityId} not found");
                
            if (tradingActivity.Status != TradingStatus.Open)
                throw new InvalidOperationException($"Trading activity with ID {tradingActivityId} is not open");
                
            // Validate take profit based on position type
            if (tradingActivity.Position == TradingPosition.Long && takeProfit <= tradingActivity.EntryPrice)
                throw new ArgumentException("For long positions, take profit must be higher than entry price");
                
            if (tradingActivity.Position == TradingPosition.Short && takeProfit >= tradingActivity.EntryPrice)
                throw new ArgumentException("For short positions, take profit must be lower than entry price");
                
            // Update take profit
            tradingActivity.TakeProfit = takeProfit;
            
            // Update in repository
            await _tradingRepository.UpdateAsync(tradingActivity);
            
            return true;
        }
        
        public async Task<List<TradingActivity>> GetOpenPositionsAsync()
        {
            return await _tradingRepository.GetByStatusAsync(TradingStatus.Open);
        }
        
        public async Task<decimal> CalculateTotalProfitLossAsync(int userId)
        {
            var activities = await _tradingRepository.GetByUserIdAsync(userId);
            
            return activities
                .Where(a => a.Status == TradingStatus.Closed)
                .Sum(a => a.ProfitLoss ?? 0);
        }
        
        public async Task<decimal> GetCurrentExposureAsync(int userId, TradingSymbol? symbol = null)
        {
            var activities = await _tradingRepository.GetByUserIdAsync(userId);
            
            var openActivities = activities
                .Where(a => a.Status == TradingStatus.Open)
                .Where(a => !symbol.HasValue || a.Symbol == symbol.Value);
                
            // Sum up the total exposure (amount * entry price)
            return openActivities.Sum(a => a.Amount * a.EntryPrice);
        }
    }
}
