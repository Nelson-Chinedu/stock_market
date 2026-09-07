using Microsoft.EntityFrameworkCore;
using dotnet_api_learning.Data;
using dotnet_api_learning.Interfaces;
using dotnet_api_learning.Models; 
using dotnet_api_learning.Dtos.Stock;



namespace dotnet_api_learning.Repository
{
    public class StockRepository: IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Stock>> GetAllAsync()
        {
            // return await _context.Stocks.ToListAsync();
            return await _context.Stocks.Include(c => c.Comments).ToListAsync();
        }

        public async Task<Stock> GetByIdAsync(int id)
        {
            // return await _context.Stocks.FindAsync(id);
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);
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