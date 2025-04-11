using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XaubotClone.Domain;
using XaubotClone.Services;

namespace XaubotClone.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradingController : ControllerBase
    {
        private readonly ITradingService _tradingService;

        public TradingController(ITradingService tradingService)
        {
            _tradingService = tradingService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TradingActivityDto>>> GetUserTradingActivities(int userId)
        {
            try
            {
                var activities = await _tradingService.GetUserTradingActivitiesAsync(userId);
                var activityDtos = activities.Select(MapToDto).ToList();
                return Ok(activityDtos);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TradingActivityDto>> GetTradingActivity(int id)
        {
            try
            {
                var activity = await _tradingService.GetTradingActivityByIdAsync(id);
                
                if (activity == null)
                    return NotFound($"Trading activity with ID {id} not found");
                    
                return Ok(MapToDto(activity));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("open")]
        public async Task<IActionResult> OpenPosition([FromBody] OpenPositionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            try
            {
                var id = await _tradingService.OpenPositionAsync(
                    request.UserId,
                    request.Symbol,
                    request.Position,
                    request.Amount,
                    request.EntryPrice,
                    request.StopLoss,
                    request.TakeProfit,
                    request.Notes
                );
                
                return CreatedAtAction(nameof(GetTradingActivity), new { id }, new { Id = id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{id}/close")]
        public async Task<IActionResult> ClosePosition(int id, [FromBody] ClosePositionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            try
            {
                var activity = await _tradingService.ClosePositionAsync(id, request.ExitPrice);
                return Ok(MapToDto(activity));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelPosition(int id)
        {
            try
            {
                var activity = await _tradingService.CancelPositionAsync(id);
                return Ok(MapToDto(activity));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/stop-loss")]
        public async Task<IActionResult> UpdateStopLoss(int id, [FromBody] UpdateStopLossRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            try
            {
                var result = await _tradingService.UpdateStopLossAsync(id, request.StopLoss);
                return Ok(new { Message = "Stop loss updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/take-profit")]
        public async Task<IActionResult> UpdateTakeProfit(int id, [FromBody] UpdateTakeProfitRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            try
            {
                var result = await _tradingService.UpdateTakeProfitAsync(id, request.TakeProfit);
                return Ok(new { Message = "Take profit updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("open")]
        public async Task<ActionResult<IEnumerable<TradingActivityDto>>> GetOpenPositions()
        {
            try
            {
                var activities = await _tradingService.GetOpenPositionsAsync();
                var activityDtos = activities.Select(MapToDto).ToList();
                return Ok(activityDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}/profit-loss")]
        public async Task<ActionResult<decimal>> GetTotalProfitLoss(int userId)
        {
            try
            {
                var profitLoss = await _tradingService.CalculateTotalProfitLossAsync(userId);
                return Ok(new { ProfitLoss = profitLoss });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}/exposure")]
        public async Task<ActionResult<decimal>> GetCurrentExposure(int userId, [FromQuery] TradingSymbol? symbol = null)
        {
            try
            {
                var exposure = await _tradingService.GetCurrentExposureAsync(userId, symbol);
                return Ok(new { Exposure = exposure });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Helper method to map TradingActivity to TradingActivityDto
        private TradingActivityDto MapToDto(TradingActivity activity)
        {
            return new TradingActivityDto
            {
                Id = activity.Id,
                UserId = activity.UserId,
                Symbol = activity.Symbol,
                Amount = activity.Amount,
                EntryPrice = activity.EntryPrice,
                ExitPrice = activity.ExitPrice,
                Position = activity.Position,
                Status = activity.Status,
                OpenTime = activity.OpenTime,
                CloseTime = activity.CloseTime,
                Notes = activity.Notes,
                StopLoss = activity.StopLoss,
                TakeProfit = activity.TakeProfit,
                ProfitLoss = activity.ProfitLoss,
                Duration = activity.Duration?.ToString() ?? null
            };
        }
    }

    // DTOs
    public class TradingActivityDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public TradingSymbol Symbol { get; set; }
        public decimal Amount { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal? ExitPrice { get; set; }
        public TradingPosition Position { get; set; }
        public TradingStatus Status { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public string Notes { get; set; }
        public decimal? StopLoss { get; set; }
        public decimal? TakeProfit { get; set; }
        public decimal? ProfitLoss { get; set; }
        public string Duration { get; set; }
    }

    public class OpenPositionRequest
    {
        public int UserId { get; set; }
        public TradingSymbol Symbol { get; set; }
        public TradingPosition Position { get; set; }
        public decimal Amount { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal? StopLoss { get; set; }
        public decimal? TakeProfit { get; set; }
        public string Notes { get; set; }
    }

    public class ClosePositionRequest
    {
        public decimal ExitPrice { get; set; }
    }

    public class UpdateStopLossRequest
    {
        public decimal StopLoss { get; set; }
    }

    public class UpdateTakeProfitRequest
    {
        public decimal TakeProfit { get; set; }
    }
}
