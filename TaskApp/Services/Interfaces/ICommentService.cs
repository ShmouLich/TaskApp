using TaskApp.Models;

namespace TaskApp.Services.Interfaces;

public interface ICommentService
{
    Task<List<Comment>> GetCommentsByTaskIdAsync(int taskId);
    Task<Comment> AddCommentAsync(Comment comment);
    Task DeleteCommentAsync(int id);
}