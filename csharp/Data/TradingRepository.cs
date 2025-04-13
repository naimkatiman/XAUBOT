using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XaubotClone.Domain;

namespace XaubotClone.Data
{
    public class TradingRepository : ITradingRepository
    {
        private readonly XaubotDbContext _context;

        public TradingRepository(XaubotDbContext context)
        {
            _context = context;
        }

        public async Task<List<TradingActivity>> GetAllAsync()
        {
            return await _context.TradingActivities
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task<TradingActivity> GetByIdAsync(int id)
        {
            return await _context.TradingActivities
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<TradingActivity>> GetByUserIdAsync(int userId)
        {
            return await _context.TradingActivities
                .Where(t => t.UserId == userId)
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task<List<TradingActivity>> GetByStatusAsync(TradingStatus status)
        {
            return await _context.TradingActivities
                .Where(t => t.Status == status)
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task<List<TradingActivity>> GetBySymbolAsync(TradingSymbol symbol)
        {
            return await _context.TradingActivities
                .Where(t => t.Symbol == symbol)
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task<int> AddAsync(TradingActivity tradingActivity)
        {
            _context.TradingActivities.Add(tradingActivity);
            await _context.SaveChangesAsync();
            return tradingActivity.Id;
        }

        public async Task UpdateAsync(TradingActivity tradingActivity)
        {
            _context.Entry(tradingActivity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tradingActivity = await GetByIdAsync(id);
            if (tradingActivity != null)
            {
                _context.TradingActivities.Remove(tradingActivity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
