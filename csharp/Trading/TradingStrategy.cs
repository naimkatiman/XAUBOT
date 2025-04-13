using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XaubotClone.Domain;
using XaubotClone.Services;

namespace XaubotClone.Trading
{
    public interface ITradingStrategy
    {
        string Name { get; }
        string Description { get; }
        Task<Dictionary<string, object>> GetParametersAsync();
        Task<StrategySignal> EvaluateAsync(TradingSymbol symbol, Dictionary<string, object> parameters);
        Task<decimal> CalculatePositionSizeAsync(int userId, TradingSymbol symbol, decimal accountBalance, decimal riskPercentage);
        Task<StrategyBacktestResult> BacktestAsync(TradingSymbol symbol, DateTime startDate, DateTime endDate, Dictionary<string, object> parameters);
    }

    public enum StrategySignalType
    {
        Buy,
        Sell,
        Hold,
        StrongBuy,
        StrongSell
    }

    public class StrategySignal
    {
        public TradingSymbol Symbol { get; set; }
        public StrategySignalType SignalType { get; set; }
        public string Reason { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal? StopLoss { get; set; }
        public decimal? TakeProfit { get; set; }
        public decimal Confidence { get; set; } // 0.0 to 1.0
        public List<string> SupportingIndicators { get; set; } = new List<string>();
        public Dictionary<string, decimal> IndicatorValues { get; set; } = new Dictionary<string, decimal>();
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

    public class StrategyBacktestResult
    {
        public TradingSymbol Symbol { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalSignals { get; set; }
        public int WinningTrades { get; set; }
        public int LosingTrades { get; set; }
        public decimal WinRate => TotalSignals > 0 ? (decimal)WinningTrades / TotalSignals : 0;
        public decimal InitialBalance { get; set; }
        public decimal FinalBalance { get; set; }
        public decimal NetProfit => FinalBalance - InitialBalance;
        public decimal ProfitFactor { get; set; }
        public decimal MaxDrawdown { get; set; }
        public List<BacktestTrade> Trades { get; set; } = new List<BacktestTrade>();
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }

    public class BacktestTrade
    {
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal? ExitPrice { get; set; }
        public TradingPosition Position { get; set; }
        public decimal Quantity { get; set; }
        public decimal? ProfitLoss { get; set; }
        public decimal? ProfitLossPercentage { get; set; }
        public string ExitReason { get; set; }
        public bool IsWinner => ProfitLoss.HasValue && ProfitLoss > 0;
    }

    // Example implementation of a Moving Average Crossover strategy
    public class MovingAverageCrossoverStrategy : ITradingStrategy
    {
        private readonly IMarketDataService _marketDataService;
        private readonly ITradingService _tradingService;

        public string Name => "Moving Average Crossover";
        public string Description => "Generates buy and sell signals based on moving average crossovers. " +
                                   "Buy signal when fast MA crosses above slow MA, sell signal when fast MA crosses below slow MA.";

        public MovingAverageCrossoverStrategy(IMarketDataService marketDataService, ITradingService tradingService)
        {
            _marketDataService = marketDataService ?? throw new ArgumentNullException(nameof(marketDataService));
            _tradingService = tradingService ?? throw new ArgumentNullException(nameof(tradingService));
        }

        public Task<Dictionary<string, object>> GetParametersAsync()
        {
            return Task.FromResult(new Dictionary<string, object>
            {
                { "FastPeriod", 10 },
                { "SlowPeriod", 50 },
                { "SignalStrengthThreshold", 0.5m }
            });
        }

        public async Task<StrategySignal> EvaluateAsync(TradingSymbol symbol, Dictionary<string, object> parameters)
        {
            // Extract parameters with defaults
            int fastPeriod = GetParameterValue<int>(parameters, "FastPeriod", 10);
            int slowPeriod = GetParameterValue<int>(parameters, "SlowPeriod", 50);
            decimal signalThreshold = GetParameterValue<decimal>(parameters, "SignalStrengthThreshold", 0.5m);

            // Validate parameters
            if (fastPeriod >= slowPeriod)
                throw new ArgumentException("Fast period must be less than slow period");

            if (fastPeriod < 2)
                throw new ArgumentException("Fast period must be at least 2");

            // Get historical data
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-slowPeriod * 2); // Get enough data for calculation
            var historicalData = await _marketDataService.GetHistoricalDataAsync(
                symbol, startDate, endDate, MarketDataInterval.Day);

            if (historicalData.Count < slowPeriod + 5)
                throw new InvalidOperationException($"Not enough historical data for {symbol}");

            // Calculate moving averages
            var closePrices = historicalData.Select(d => d.ClosePrice).ToList();
            var fastMA = CalculateSimpleMovingAverage(closePrices, fastPeriod);
            var slowMA = CalculateSimpleMovingAverage(closePrices, slowPeriod);

            // Get current and previous values
            decimal currentFastMA = fastMA.Last();
            decimal currentSlowMA = slowMA.Last();
            decimal previousFastMA = fastMA[fastMA.Count - 2];
            decimal previousSlowMA = slowMA[slowMA.Count - 2];

            // Get current price
            decimal currentPrice = await _marketDataService.GetCurrentPriceAsync(symbol);

            // Determine signal
            StrategySignalType signalType;
            string reason;
            decimal confidence;

            if (currentFastMA > currentSlowMA && previousFastMA <= previousSlowMA)
            {
                // Bullish crossover
                signalType = StrategySignalType.Buy;
                reason = "Bullish crossover: Fast MA crossed above Slow MA";
                confidence = CalculateSignalConfidence(currentFastMA, currentSlowMA, signalThreshold);
                
                if (confidence > 0.8m)
                    signalType = StrategySignalType.StrongBuy;
            }
            else if (currentFastMA < currentSlowMA && previousFastMA >= previousSlowMA)
            {
                // Bearish crossover
                signalType = StrategySignalType.Sell;
                reason = "Bearish crossover: Fast MA crossed below Slow MA";
                confidence = CalculateSignalConfidence(currentSlowMA, currentFastMA, signalThreshold);
                
                if (confidence > 0.8m)
                    signalType = StrategySignalType.StrongSell;
            }
            else
            {
                // No crossover
                signalType = StrategySignalType.Hold;
                reason = "No MA crossover detected";
                confidence = 0.1m;
            }

            // Calculate stop loss and take profit levels
            decimal? stopLoss = null;
            decimal? takeProfit = null;

            if (signalType == StrategySignalType.Buy || signalType == StrategySignalType.StrongBuy)
            {
                // Stop loss: 2% below entry price
                stopLoss = Math.Round(currentPrice * 0.98m, 2);
                // Take profit: 4% above entry price
                takeProfit = Math.Round(currentPrice * 1.04m, 2);
            }
            else if (signalType == StrategySignalType.Sell || signalType == StrategySignalType.StrongSell)
            {
                // Stop loss: 2% above entry price
                stopLoss = Math.Round(currentPrice * 1.02m, 2);
                // Take profit: 4% below entry price
                takeProfit = Math.Round(currentPrice * 0.96m, 2);
            }

            // Create the signal
            var signal = new StrategySignal
            {
                Symbol = symbol,
                SignalType = signalType,
                Reason = reason,
                EntryPrice = currentPrice,
                StopLoss = stopLoss,
                TakeProfit = takeProfit,
                Confidence = confidence,
                GeneratedAt = DateTime.UtcNow,
                SupportingIndicators = new List<string> { "Simple Moving Average" },
                IndicatorValues = new Dictionary<string, decimal>
                {
                    { $"SMA{fastPeriod}", currentFastMA },
                    { $"SMA{slowPeriod}", currentSlowMA }
                }
            };

            return signal;
        }

        public async Task<decimal> CalculatePositionSizeAsync(int userId, TradingSymbol symbol, decimal accountBalance, decimal riskPercentage)
        {
            if (riskPercentage <= 0 || riskPercentage > 5)
                throw new ArgumentException("Risk percentage must be between 0 and 5 percent");

            var signal = await EvaluateAsync(symbol, await GetParametersAsync());
            
            if (signal.SignalType == StrategySignalType.Hold)
                return 0;
                
            if (!signal.StopLoss.HasValue)
                throw new InvalidOperationException("Stop loss is required for position sizing");

            // Calculate risk amount
            decimal riskAmount = accountBalance * (riskPercentage / 100);
            
            // Calculate risk per unit
            decimal riskPerUnit;
            if (signal.SignalType == StrategySignalType.Buy || signal.SignalType == StrategySignalType.StrongBuy)
            {
                riskPerUnit = signal.EntryPrice - signal.StopLoss.Value;
            }
            else
            {
                riskPerUnit = signal.StopLoss.Value - signal.EntryPrice;
            }
            
            if (riskPerUnit <= 0)
                throw new InvalidOperationException("Invalid risk per unit calculation");
                
            // Calculate position size
            decimal positionSize = riskAmount / riskPerUnit;
            
            // Round down to 2 decimal places
            return Math.Floor(positionSize * 100) / 100;
        }

        public async Task<StrategyBacktestResult> BacktestAsync(TradingSymbol symbol, DateTime startDate, DateTime endDate, Dictionary<string, object> parameters)
        {
            // Extract parameters with defaults
            int fastPeriod = GetParameterValue<int>(parameters, "FastPeriod", 10);
            int slowPeriod = GetParameterValue<int>(parameters, "SlowPeriod", 50);

            // Get historical data for the period
            var historicalData = await _marketDataService.GetHistoricalDataAsync(
                symbol, startDate, endDate, MarketDataInterval.Day);

            if (historicalData.Count < slowPeriod + 5)
                throw new InvalidOperationException($"Not enough historical data for {symbol}");

            // Calculate moving averages for the entire period
            var closePrices = historicalData.Select(d => d.ClosePrice).ToList();
            var dates = historicalData.Select(d => d.Timestamp).ToList();
            
            var fastMA = CalculateSimpleMovingAverage(closePrices, fastPeriod);
            var slowMA = CalculateSimpleMovingAverage(closePrices, slowPeriod);

            // Initialize backtest parameters
            const decimal initialBalance = 10000;
            decimal balance = initialBalance;
            decimal maxBalance = initialBalance;
            decimal drawdown = 0;
            decimal maxDrawdown = 0;
            decimal totalProfit = 0;
            decimal totalLoss = 0;
            
            var trades = new List<BacktestTrade>();
            BacktestTrade currentTrade = null;
            
            // Start backtesting from the point where we have enough data for both MAs
            for (int i = slowPeriod; i < closePrices.Count - 1; i++)
            {
                decimal currentFastMA = fastMA[i];
                decimal currentSlowMA = slowMA[i];
                decimal previousFastMA = fastMA[i - 1];
                decimal previousSlowMA = slowMA[i - 1];
                
                decimal currentPrice = closePrices[i];
                decimal nextPrice = closePrices[i + 1]; // Next price to simulate execution
                DateTime currentDate = dates[i];
                
                // Check for entry signals
                if (currentTrade == null)
                {
                    // Buy signal: Fast MA crosses above Slow MA
                    if (currentFastMA > currentSlowMA && previousFastMA <= previousSlowMA)
                    {
                        // Calculate position size (10% of balance)
                        decimal positionSize = balance * 0.1m;
                        decimal quantity = positionSize / nextPrice;
                        
                        currentTrade = new BacktestTrade
                        {
                            EntryTime = currentDate,
                            EntryPrice = nextPrice,
                            Position = TradingPosition.Long,
                            Quantity = quantity
                        };
                    }
                    // Sell signal: Fast MA crosses below Slow MA
                    else if (currentFastMA < currentSlowMA && previousFastMA >= previousSlowMA)
                    {
                        // Calculate position size (10% of balance)
                        decimal positionSize = balance * 0.1m;
                        decimal quantity = positionSize / nextPrice;
                        
                        currentTrade = new BacktestTrade
                        {
                            EntryTime = currentDate,
                            EntryPrice = nextPrice,
                            Position = TradingPosition.Short,
                            Quantity = quantity
                        };
                    }
                }
                // Check for exit signals
                else
                {
                    bool shouldExit = false;
                    string exitReason = "";
                    
                    // Exit long on bearish crossover
                    if (currentTrade.Position == TradingPosition.Long && 
                        currentFastMA < currentSlowMA && previousFastMA >= previousSlowMA)
                    {
                        shouldExit = true;
                        exitReason = "Exit long position on bearish crossover";
                    }
                    // Exit short on bullish crossover
                    else if (currentTrade.Position == TradingPosition.Short && 
                             currentFastMA > currentSlowMA && previousFastMA <= previousSlowMA)
                    {
                        shouldExit = true;
                        exitReason = "Exit short position on bullish crossover";
                    }
                    
                    // Process exit
                    if (shouldExit)
                    {
                        currentTrade.ExitTime = currentDate;
                        currentTrade.ExitPrice = nextPrice;
                        currentTrade.ExitReason = exitReason;
                        
                        // Calculate profit/loss
                        if (currentTrade.Position == TradingPosition.Long)
                        {
                            currentTrade.ProfitLoss = (nextPrice - currentTrade.EntryPrice) * currentTrade.Quantity;
                        }
                        else
                        {
                            currentTrade.ProfitLoss = (currentTrade.EntryPrice - nextPrice) * currentTrade.Quantity;
                        }
                        
                        currentTrade.ProfitLossPercentage = (currentTrade.ProfitLoss.Value / (currentTrade.EntryPrice * currentTrade.Quantity)) * 100;
                        
                        // Update balance
                        balance += currentTrade.ProfitLoss.Value;
                        
                        // Track max balance and drawdown
                        if (balance > maxBalance)
                        {
                            maxBalance = balance;
                            drawdown = 0;
                        }
                        else
                        {
                            drawdown = (maxBalance - balance) / maxBalance;
                            maxDrawdown = Math.Max(maxDrawdown, drawdown);
                        }
                        
                        // Track profit/loss totals
                        if (currentTrade.ProfitLoss.Value > 0)
                            totalProfit += currentTrade.ProfitLoss.Value;
                        else
                            totalLoss -= currentTrade.ProfitLoss.Value;
                        
                        trades.Add(currentTrade);
                        currentTrade = null;
                    }
                }
            }
            
            // Close any open trades at the end of the period
            if (currentTrade != null)
            {
                currentTrade.ExitTime = dates.Last();
                currentTrade.ExitPrice = closePrices.Last();
                currentTrade.ExitReason = "End of backtest period";
                
                // Calculate profit/loss
                if (currentTrade.Position == TradingPosition.Long)
                {
                    currentTrade.ProfitLoss = (currentTrade.ExitPrice.Value - currentTrade.EntryPrice) * currentTrade.Quantity;
                }
                else
                {
                    currentTrade.ProfitLoss = (currentTrade.EntryPrice - currentTrade.ExitPrice.Value) * currentTrade.Quantity;
                }
                
                currentTrade.ProfitLossPercentage = (currentTrade.ProfitLoss.Value / (currentTrade.EntryPrice * currentTrade.Quantity)) * 100;
                
                // Update balance
                balance += currentTrade.ProfitLoss.Value;
                
                if (currentTrade.ProfitLoss.Value > 0)
                    totalProfit += currentTrade.ProfitLoss.Value;
                else
                    totalLoss -= currentTrade.ProfitLoss.Value;
                
                trades.Add(currentTrade);
            }
            
            // Calculate profit factor
            decimal profitFactor = totalLoss > 0 ? totalProfit / totalLoss : totalProfit;
            
            return new StrategyBacktestResult
            {
                Symbol = symbol,
                StartDate = startDate,
                EndDate = endDate,
                TotalSignals = trades.Count,
                WinningTrades = trades.Count(t => t.IsWinner),
                LosingTrades = trades.Count(t => !t.IsWinner),
                InitialBalance = initialBalance,
                FinalBalance = balance,
                ProfitFactor = profitFactor,
                MaxDrawdown = maxDrawdown * 100, // Convert to percentage
                Trades = trades,
                Parameters = parameters
            };
        }

        // Helper methods
        private List<decimal> CalculateSimpleMovingAverage(List<decimal> prices, int period)
        {
            if (prices.Count < period)
                throw new ArgumentException("Not enough price data for the specified period");

            var result = new List<decimal>();
            
            // Calculate SMA - first period-1 entries will be empty
            for (int i = 0; i < period - 1; i++)
            {
                result.Add(0); // Placeholder
            }
            
            // Calculate actual SMA values
            for (int i = period - 1; i < prices.Count; i++)
            {
                decimal sum = 0;
                for (int j = 0; j < period; j++)
                {
                    sum += prices[i - j];
                }
                
                result.Add(sum / period);
            }
            
            return result;
        }

        private decimal CalculateSignalConfidence(decimal higherValue, decimal lowerValue, decimal threshold)
        {
            // Calculate the percentage difference between the two values
            var percentDiff = (higherValue - lowerValue) / lowerValue;
            
            // Scale the confidence based on the threshold
            var confidence = Math.Min(percentDiff / threshold, 1.0m);
            
            return Math.Max(confidence, 0.1m);
        }

        private T GetParameterValue<T>(Dictionary<string, object> parameters, string key, T defaultValue)
        {
            if (parameters == null || !parameters.ContainsKey(key))
                return defaultValue;
            
            try
            {
                return (T)Convert.ChangeType(parameters[key], typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
