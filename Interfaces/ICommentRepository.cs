using dotnet_api_learning.Models; 


namespace dotnet_api_learning.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();

        Task<Comment?> GetByIdAsync(int id);
    }
}