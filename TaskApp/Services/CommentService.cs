using Microsoft.EntityFrameworkCore;
using TaskApp.Data;
using TaskApp.Models;
using TaskApp.Services.Interfaces;

namespace TaskApp.Services;

public class CommentService : ICommentService
{
    
    private readonly TaskAppDbContext _dbContext;
    
    public CommentService(TaskAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Comment>> GetCommentsByTaskIdAsync(int taskId)
    { 
        return await _dbContext.Comments
            .Where(c => c.TaskItemId == taskId)
            .OrderBy(c => c.Created)
            .ToListAsync();
    }

    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        _dbContext.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();
        
        return comment;
    }

    public async Task DeleteCommentAsync(int id)
    {
        var item = await _dbContext.Comments.FindAsync(id);

        if (item != null)
        {
            _dbContext.Comments.Remove(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}