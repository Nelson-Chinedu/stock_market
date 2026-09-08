using dotnet_api_learning.Models;

namespace dotnet_api_learning.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}