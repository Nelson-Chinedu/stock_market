using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using  dotnet_api_learning.Helpers;
using dotnet_api_learning.Data;
using dotnet_api_learning.Mappers;
using dotnet_api_learning.Dtos.Stock;
using dotnet_api_learning.Interfaces;

namespace dotnet_api_learning.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController: ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _context = context;
            _stockRepo = stockRepo;
        }

        // synchronous code
        // [HttpGet]
        // public IActionResult GetAll()
        // {
        //     var stocks = _context.Stocks.ToList().Select(s => s.ToStockDto());

        //     return Ok(stocks);
        // }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            // var stocks = await _context.Stocks.ToListAsync();
            var stocks = await _stockRepo.GetAllAsync(query);
            // var stockDto = stocks.Data.Select(s => s.ToStockDto());

            // var response = new PagedResponse<StockDto>(
            //     stockDtos, 
            //     pagedStocks.TotalRecords, 
            //     pagedStocks.PageNumber, 
            //     pagedStocks.PageSize
            // );

            // return Ok(response);
            // Cleanly map Stock -> StockDto using the helper method
            var response = stocks.Map(s => s.ToStockDto());

            return Ok(response);
        }


        // synchronous code
        // [HttpGet("{id}")]
        // public IActionResult GetById([FromRoute] int id)
        // {
        //     var stock = _context.Stocks.Find(id);

        //     if(stock == null)
        //     {
        //         return NotFound(new {message = $"Stock with id {id} not found"});
        //     }

        //     return Ok(stock.ToStockDto());
        // }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var stock = await _stockRepo.GetByIdAsync(id);

            if(stock == null)
            {
                return NotFound(new {message = $"Stock with id {id} not found"});
            }

            return Ok(stock.ToStockDto());
        }


        // synchronous code
        // [HttpPost]
        // public IActionResult CreateStock([FromBody] CreateStockRequestDto stockDto)
        // {
        //     var stockModel = stockDto.ToStockFromCreateDto();
        //     _context.Stocks.Add(stockModel);
        //     _context.SaveChanges();
        //     return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
        // }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var stockModel = stockDto.ToStockFromCreateDto();
            // await _context.Stocks.AddAsync(stockModel);
            // await _context.SaveChangesAsync();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
        }

        // synchronous code
        // [HttpPut]
        // [Route("{id}")]
        // public IActionResult UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        // {
        //     var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);

        //     if(stockModel == null)
        //     {
        //         return NotFound();
        //     }

        //     stockModel.Symbol = updateDto.Symbol;
        //     stockModel.CompanyName = updateDto.CompanyName;
        //     stockModel.Purchase = updateDto.Purchase;
        //     stockModel.LastDiv = updateDto.LastDiv;
        //     stockModel.Industry = updateDto.Industry;
        //     stockModel.MarketCap = updateDto.MarketCap;

        //     _context.SaveChanges();
        //     return Ok(stockModel.ToStockDto());
        // }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            // var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            var stockModel = await _stockRepo.UpdateAsync(id, updateDto);


            if(stockModel == null)
            {
                return NotFound();
            }

            // stockModel.Symbol = updateDto.Symbol;
            // stockModel.CompanyName = updateDto.CompanyName;
            // stockModel.Purchase = updateDto.Purchase;
            // stockModel.LastDiv = updateDto.LastDiv;
            // stockModel.Industry = updateDto.Industry;
            // stockModel.MarketCap = updateDto.MarketCap;

            // await _context.SaveChangesAsync();
            return Ok(stockModel.ToStockDto());
        }


        // synchronous code
        // [HttpDelete]
        // [Route("{id}")]
        // public IActionResult DeleteStock([FromRoute] int id)
        // {
        //     var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);

        //     if(stockModel == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Stocks.Remove(stockModel);
        //     _context.SaveChanges();
        //     return NoContent();
        // }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                            // var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            var stockModel = await _stockRepo.DeleteAsync(id);

            if(stockModel == null)
            {
                return NotFound();
            }

            // _context.Stocks.Remove(stockModel);
            // await _context.SaveChangesAsync();
            return NoContent();                
            }
            catch (DbUpdateException)
            {
                
                return BadRequest("Cannot delete this stock because it has associated comments.");
            }

        }

        
    }
}