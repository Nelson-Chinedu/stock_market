using Microsoft.EntityFrameworkCore;
using dotnet_api_learning.Data;
using dotnet_api_learning.Interfaces;
using dotnet_api_learning.Models; 


namespace dotnet_api_learning.Repository
{
    public class CommentRepository: ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var commentExist = await _context.Comments.FindAsync(id);

            if(commentExist == null)
            {
                return null;
            }

            commentExist.Title = commentModel.Title;
            commentExist.Content = commentModel.Content;

            await _context.SaveChangesAsync();
            
            return commentExist;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentExist = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            
            if(commentExist == null)
            {
                return null;
            }

            _context.Comments.Remove(commentExist);
            await _context.SaveChangesAsync();
            return commentExist;
        }
    }
}