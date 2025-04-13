using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XaubotClone.Domain;
using XaubotClone.Data;
using XaubotClone.Services;

namespace XaubotClone.Services
{
    public interface IRiskManagementService
    {
        Task<RiskAssessment> CalculatePositionRiskAsync(TradingActivity position);
        Task<decimal> CalculateMaxPositionSizeAsync(int userId, TradingSymbol symbol, TradingPosition positionType, decimal maxRiskPercent);
        Task<Dictionary<TradingSymbol, decimal>> GetExposureBySymbolAsync(int userId);
        Task<bool> IsWithinRiskLimitsAsync(int userId, TradingSymbol symbol, decimal amount);
        Task<PortfolioRiskReport> GeneratePortfolioRiskReportAsync(int userId);
        Task<RiskProfile> GetUserRiskProfileAsync(int userId);
        Task<bool> UpdateUserRiskProfileAsync(int userId, RiskProfile riskProfile);
        Task<List<TradingActivity>> IdentifyRiskyPositionsAsync(int userId);
    }

    public class RiskManagementService : IRiskManagementService
    {
        private readonly ITradingRepository _tradingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMarketDataService _marketDataService;

        public RiskManagementService(
            ITradingRepository tradingRepository,
            IUserRepository userRepository,
            IMarketDataService marketDataService)
        {
            _tradingRepository = tradingRepository ?? throw new ArgumentNullException(nameof(tradingRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _marketDataService = marketDataService ?? throw new ArgumentNullException(nameof(marketDataService));
        }

        public async Task<RiskAssessment> CalculatePositionRiskAsync(TradingActivity position)
        {
            if (position == null)
                throw new ArgumentNullException(nameof(position));

            if (position.Status != TradingStatus.Open)
                throw new ArgumentException("Can only calculate risk for open positions");

            // Get current price
            decimal currentPrice = await _marketDataService.GetCurrentPriceAsync(position.Symbol);

            // Calculate current P/L
            decimal profitLoss = position.CalculateProfitLoss(currentPrice);
            decimal profitLossPercent = position.CalculateProfitLossPercent(currentPrice);

            // Calculate stop loss risk if stop loss exists
            decimal stopLossRisk = 0;
            decimal maxLoss = 0;
            
            if (position.StopLoss.HasValue)
            {
                stopLossRisk = position.Position == TradingPosition.Long
                    ? Math.Abs((position.StopLoss.Value - position.EntryPrice) / position.EntryPrice)
                    : Math.Abs((position.EntryPrice - position.StopLoss.Value) / position.EntryPrice);
                
                maxLoss = stopLossRisk * position.Amount;
            }
            else
            {
                // Assume 100% risk if no stop loss
                stopLossRisk = 1.0m;
                maxLoss = position.Amount;
            }

            // Calculate risk-to-reward ratio if take profit exists
            decimal? riskRewardRatio = null;
            if (position.StopLoss.HasValue && position.TakeProfit.HasValue)
            {
                decimal potentialLoss = Math.Abs(position.EntryPrice - position.StopLoss.Value);
                decimal potentialGain = Math.Abs(position.TakeProfit.Value - position.EntryPrice);
                
                if (potentialLoss > 0)
                {
                    riskRewardRatio = potentialGain / potentialLoss;
                }
            }

            return new RiskAssessment
            {
                PositionId = position.Id,
                Symbol = position.Symbol,
                CurrentPrice = currentPrice,
                EntryPrice = position.EntryPrice,
                StopLoss = position.StopLoss,
                TakeProfit = position.TakeProfit,
                PositionSize = position.Amount,
                CurrentProfitLoss = profitLoss,
                CurrentProfitLossPercent = profitLossPercent,
                StopLossRiskPercent = stopLossRisk * 100, // Convert to percentage
                MaxLossAmount = maxLoss,
                RiskRewardRatio = riskRewardRatio,
                IsWithinRiskLimits = stopLossRisk <= 0.02m // 2% max risk per position
            };
        }

        public async Task<decimal> CalculateMaxPositionSizeAsync(
            int userId,
            TradingSymbol symbol,
            TradingPosition positionType,
            decimal maxRiskPercent)
        {
            if (maxRiskPercent <= 0 || maxRiskPercent > 10)
                throw new ArgumentException("Max risk percentage must be between 0 and 10 percent");

            // Determine total account value
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");

            // This would normally be calculated from account balance or fetched from a repository
            // For this example, we'll use a placeholder value
            decimal accountValue = 10000; // TODO: Get actual account value
            
            // Calculate risk amount
            decimal riskAmount = accountValue * (maxRiskPercent / 100);

            // Get current price
            decimal currentPrice = await _marketDataService.GetCurrentPriceAsync(symbol);
            
            // Calculate max position size based on 2% stop loss
            decimal stopLossDistance = currentPrice * 0.02m; // 2% stop loss
            
            // Calculate position size
            decimal maxPositionSize = riskAmount / stopLossDistance;
            
            return Math.Round(maxPositionSize, 2);
        }

        public async Task<Dictionary<TradingSymbol, decimal>> GetExposureBySymbolAsync(int userId)
        {
            // Get all open positions for the user
            var positions = await _tradingRepository.GetByUserIdAsync(userId);
            var openPositions = positions.Where(p => p.Status == TradingStatus.Open).ToList();
            
            // Calculate exposure by symbol
            var exposureBySymbol = new Dictionary<TradingSymbol, decimal>();
            
            foreach (var position in openPositions)
            {
                if (!exposureBySymbol.ContainsKey(position.Symbol))
                {
                    exposureBySymbol[position.Symbol] = 0;
                }
                
                exposureBySymbol[position.Symbol] += position.Amount;
            }
            
            return exposureBySymbol;
        }

        public async Task<bool> IsWithinRiskLimitsAsync(int userId, TradingSymbol symbol, decimal amount)
        {
            // Get existing exposure
            var exposureBySymbol = await GetExposureBySymbolAsync(userId);
            
            // Get total account value
            decimal accountValue = 10000; // TODO: Get actual account value
            
            // Calculate current exposure for the symbol
            decimal currentExposure = exposureBySymbol.ContainsKey(symbol) ? exposureBySymbol[symbol] : 0;
            
            // Calculate total exposure with new position
            decimal totalExposure = currentExposure + amount;
            
            // Check risk limits:
            // 1. Maximum 20% of account in a single symbol
            // 2. Maximum 50% of account across all positions
            bool withinSymbolLimit = totalExposure <= (accountValue * 0.2m);
            
            decimal totalPositionsExposure = exposureBySymbol.Values.Sum() + amount;
            bool withinTotalLimit = totalPositionsExposure <= (accountValue * 0.5m);
            
            return withinSymbolLimit && withinTotalLimit;
        }

        public async Task<PortfolioRiskReport> GeneratePortfolioRiskReportAsync(int userId)
        {
            // Get open positions
            var positions = await _tradingRepository.GetByUserIdAsync(userId);
            var openPositions = positions.Where(p => p.Status == TradingStatus.Open).ToList();
            
            // Calculate risk metrics for each position
            var positionRisks = new List<RiskAssessment>();
            foreach (var position in openPositions)
            {
                var risk = await CalculatePositionRiskAsync(position);
                positionRisks.Add(risk);
            }
            
            // Calculate portfolio metrics
            decimal totalExposure = openPositions.Sum(p => p.Amount);
            decimal totalRisk = positionRisks.Sum(r => r.MaxLossAmount);
            
            // Placeholder for account value
            decimal accountValue = 10000; // TODO: Get actual account value
            
            return new PortfolioRiskReport
            {
                UserId = userId,
                AccountValue = accountValue,
                TotalExposure = totalExposure,
                ExposurePercent = (totalExposure / accountValue) * 100,
                TotalRisk = totalRisk,
                TotalRiskPercent = (totalRisk / accountValue) * 100,
                MaxDrawdownAmount = totalRisk,
                MaxDrawdownPercent = (totalRisk / accountValue) * 100,
                PositionRisks = positionRisks,
                GeneratedAt = DateTime.UtcNow
            };
        }

        public async Task<RiskProfile> GetUserRiskProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");
            
            // Check if user has a risk profile stored in preferences
            var riskProfileJson = user.GetPreference("RiskProfile");
            
            if (string.IsNullOrWhiteSpace(riskProfileJson))
            {
                // Create a default risk profile
                return new RiskProfile
                {
                    UserId = userId,
                    MaxRiskPerTrade = 2, // 2% per trade
                    MaxRiskTotal = 10, // 10% total portfolio risk
                    MaxExposurePerSymbol = 20, // 20% per symbol
                    MaxExposureTotal = 50, // 50% total exposure
                    DefaultStopLossPercent = 2, // 2% default stop loss
                    DefaultTakeProfitPercent = 4, // 4% default take profit
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            }
            
            // TODO: Deserialize JSON to RiskProfile object
            // For now, return a placeholder
            return new RiskProfile
            {
                UserId = userId,
                MaxRiskPerTrade = 2,
                MaxRiskTotal = 10,
                MaxExposurePerSymbol = 20,
                MaxExposureTotal = 50,
                DefaultStopLossPercent = 2,
                DefaultTakeProfitPercent = 4,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public async Task<bool> UpdateUserRiskProfileAsync(int userId, RiskProfile riskProfile)
        {
            if (riskProfile == null)
                throw new ArgumentNullException(nameof(riskProfile));
                
            if (riskProfile.UserId != userId)
                throw new ArgumentException("Risk profile user ID does not match");
                
            // Validate risk limits
            if (riskProfile.MaxRiskPerTrade < 0.1m || riskProfile.MaxRiskPerTrade > 10)
                throw new ArgumentException("Max risk per trade must be between 0.1% and 10%");
                
            if (riskProfile.MaxRiskTotal < 1 || riskProfile.MaxRiskTotal > 50)
                throw new ArgumentException("Max total risk must be between 1% and 50%");
                
            // Get user
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");
                
            // Update risk profile in user preferences
            // TODO: Serialize RiskProfile to JSON
            string riskProfileJson = "{}"; // Placeholder
            
            user.SetPreference("RiskProfile", riskProfileJson);
            await _userRepository.UpdateAsync(user);
            
            return true;
        }

        public async Task<List<TradingActivity>> IdentifyRiskyPositionsAsync(int userId)
        {
            // Get user's risk profile
            var riskProfile = await GetUserRiskProfileAsync(userId);
            
            // Get open positions
            var positions = await _tradingRepository.GetByUserIdAsync(userId);
            var openPositions = positions.Where(p => p.Status == TradingStatus.Open).ToList();
            
            // Identify risky positions
            var riskyPositions = new List<TradingActivity>();
            
            foreach (var position in openPositions)
            {
                var risk = await CalculatePositionRiskAsync(position);
                
                // Check if position exceeds risk limits
                if (risk.StopLossRiskPercent > riskProfile.MaxRiskPerTrade || !position.StopLoss.HasValue)
                {
                    riskyPositions.Add(position);
                }
            }
            
            return riskyPositions;
        }
    }

    public class RiskAssessment
    {
        public int PositionId { get; set; }
        public TradingSymbol Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal? StopLoss { get; set; }
        public decimal? TakeProfit { get; set; }
        public decimal PositionSize { get; set; }
        public decimal CurrentProfitLoss { get; set; }
        public decimal CurrentProfitLossPercent { get; set; }
        public decimal StopLossRiskPercent { get; set; }
        public decimal MaxLossAmount { get; set; }
        public decimal? RiskRewardRatio { get; set; }
        public bool IsWithinRiskLimits { get; set; }
    }

    public class RiskProfile
    {
        public int UserId { get; set; }
        public decimal MaxRiskPerTrade { get; set; } // Percentage
        public decimal MaxRiskTotal { get; set; } // Percentage
        public decimal MaxExposurePerSymbol { get; set; } // Percentage
        public decimal MaxExposureTotal { get; set; } // Percentage
        public decimal DefaultStopLossPercent { get; set; }
        public decimal DefaultTakeProfitPercent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PortfolioRiskReport
    {
        public int UserId { get; set; }
        public decimal AccountValue { get; set; }
        public decimal TotalExposure { get; set; }
        public decimal ExposurePercent { get; set; }
        public decimal TotalRisk { get; set; }
        public decimal TotalRiskPercent { get; set; }
        public decimal MaxDrawdownAmount { get; set; }
        public decimal MaxDrawdownPercent { get; set; }
        public List<RiskAssessment> PositionRisks { get; set; } = new List<RiskAssessment>();
        public DateTime GeneratedAt { get; set; }
    }
}
