using System.ComponentModel.DataAnnotations;

namespace dotnet_api_learning.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}