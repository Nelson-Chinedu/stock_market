using dotnet_api_learning.Models;

namespace dotnet_api_learning.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
    }
}