using Microsoft.AspNetCore.Mvc;
using dotnet_api_learning.Interfaces;
using dotnet_api_learning.Mappers;
using dotnet_api_learning.Dtos.Comment;


namespace dotnet_api_learning.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController: ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync(); 
            var commentDto = comments.Select(s => s.ToCommentDto());
            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var comment = await _commentRepo.GetByIdAsync(id);

            if(comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            // this checks to see if stock id exists before creating a comment for it
            if(!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("Stock does not exist");
            }

            var commentModel = commentDto.ToCommentFromCreate(stockId);
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateCommentDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var comment = await _commentRepo.UpdateAsync(id, updateCommentDto.ToCommentFromUpdate());

            if(comment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var comment = await _commentRepo.DeleteAsync(id);

            if(comment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(comment);
        }
        
    }
}