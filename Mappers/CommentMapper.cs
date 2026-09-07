using dotnet_api_learning.Models;
using dotnet_api_learning.Dtos.Stock;
using dotnet_api_learning.Dtos.Comment;

namespace dotnet_api_learning.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId
            };
        }
    }
}