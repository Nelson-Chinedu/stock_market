using System.ComponentModel.DataAnnotations;

namespace dotnet_api_learning.Dtos.Comment
{
    public class CreateCommentDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content must be at least 5 characters long")]
        [MaxLength(200, ErrorMessage = "Content cannot exceed 200 characters")]
        public string Content { get; set; } = string.Empty;
    }
}