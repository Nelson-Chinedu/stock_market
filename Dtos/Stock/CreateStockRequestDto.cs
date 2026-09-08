using System.ComponentModel.DataAnnotations;
namespace dotnet_api_learning.Dtos.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot exceed 10 characters")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MinLength(2, ErrorMessage = "CompanyName must be at least 2 characters long")]
        [MaxLength(100, ErrorMessage = "CompanyName cannot exceed 100 characters")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Industry cannot exceed 100 characters")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1, 5000000)]
        public long MarketCap { get; set; }
    }
}