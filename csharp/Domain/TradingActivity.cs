using System;
using System.ComponentModel.DataAnnotations;

namespace XaubotClone.Domain
{
    public class TradingActivity
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        [Required]
        public TradingSymbol Symbol { get; set; }
        
        public decimal Amount { get; set; }
        
        public decimal EntryPrice { get; set; }
        
        public decimal? ExitPrice { get; set; }
        
        public TradingPosition Position { get; set; }
        
        public TradingStatus Status { get; set; }
        
        public DateTime OpenTime { get; set; } = DateTime.UtcNow;
        
        public DateTime? CloseTime { get; set; }
        
        [StringLength(500)]
        public string Notes { get; set; }
        
        public decimal? StopLoss { get; set; }
        
        public decimal? TakeProfit { get; set; }
        
        public User User { get; set; }
        
        public decimal? ProfitLoss => CalculateProfitLoss();
        
        public TimeSpan? Duration => CloseTime.HasValue ? CloseTime.Value - OpenTime : null;
        
        private decimal? CalculateProfitLoss()
        {
            if (!ExitPrice.HasValue || Status == TradingStatus.Open)
                return null;
                
            if (Position == TradingPosition.Long)
                return Amount * (ExitPrice.Value - EntryPrice);
            else
                return Amount * (EntryPrice - ExitPrice.Value);
        }
        
        public void Close(decimal exitPrice)
        {
            ExitPrice = exitPrice;
            CloseTime = DateTime.UtcNow;
            Status = TradingStatus.Closed;
        }
        
        public void Cancel()
        {
            Status = TradingStatus.Cancelled;
            CloseTime = DateTime.UtcNow;
        }
        
        public bool IsProfit()
        {
            if (!ProfitLoss.HasValue)
                return false;
                
            return ProfitLoss.Value > 0;
        }
        
        public decimal GetCurrentProfitLoss(decimal currentPrice)
        {
            if (Position == TradingPosition.Long)
                return Amount * (currentPrice - EntryPrice);
            else
                return Amount * (EntryPrice - currentPrice);
        }
        
        public bool ShouldTriggerStopLoss(decimal currentPrice)
        {
            if (!StopLoss.HasValue)
                return false;
                
            if (Position == TradingPosition.Long)
                return currentPrice <= StopLoss.Value;
            else
                return currentPrice >= StopLoss.Value;
        }
        
        public bool ShouldTriggerTakeProfit(decimal currentPrice)
        {
            if (!TakeProfit.HasValue)
                return false;
                
            if (Position == TradingPosition.Long)
                return currentPrice >= TakeProfit.Value;
            else
                return currentPrice <= TakeProfit.Value;
        }
    }
}
