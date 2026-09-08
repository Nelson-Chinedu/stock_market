using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using dotnet_api_learning.Extensions;
using dotnet_api_learning.Interfaces;
using dotnet_api_learning.Models;
using dotnet_api_learning.Extensions;

namespace dotnet_api_learning.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController: ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            // if (string.IsNullOrEmpty(username))
            // {
            //     return BadRequest("Username claim could not be resolved from JWT.");
            // }

            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            return Ok(userPortfolio);
        }
    }
}