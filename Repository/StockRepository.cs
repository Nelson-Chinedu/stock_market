using Microsoft.EntityFrameworkCore;
using dotnet_api_learning.Data;
using dotnet_api_learning.Interfaces;
using dotnet_api_learning.Models; 
using dotnet_api_learning.Dtos.Stock;
using dotnet_api_learning.Helpers;


namespace dotnet_api_learning.Repository
{
    public class StockRepository: IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public Task<bool> StockExists(int id)
        {
            return _context.Stocks.AnyAsync(s => s.Id == id);
        }
        public async Task<PagedResponse<Stock>> GetAllAsync(QueryObject query)
        {
            // return await _context.Stocks.ToListAsync();
            // return await _context.Stocks.Include(c => c.Comments).ToListAsync();
            
            var stocks = _context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                    
                }
            }

            var totalRecords = await stocks.CountAsync();

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            var items = await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();

            return new PagedResponse<Stock>(items, totalRecords, query.PageNumber, query.PageSize);
        }

        public async Task<Stock> GetByIdAsync(int id)
        {
            // return await _context.Stocks.FindAsync(id);
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stockExist = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if(stockExist == null)
            {
                return null;
            }

            stockExist.Symbol = stockDto.Symbol;
            stockExist.CompanyName = stockDto.CompanyName;
            stockExist.Purchase = stockDto.Purchase;
            stockExist.LastDiv = stockDto.LastDiv;
            stockExist.Industry = stockDto.Industry;
            stockExist.MarketCap = stockDto.MarketCap;

            await _context.SaveChangesAsync();
            return stockExist;
        }

        public async Task<Stock> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if(stockModel == null)
            {
                return null;
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

    }
}