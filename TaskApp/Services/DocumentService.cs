using Microsoft.EntityFrameworkCore;
using TaskApp.Data;
using TaskApp.Models;
using TaskApp.Services.Interfaces;

namespace TaskApp.Services;

public class DocumentService : IDocumentService
{
    
    private readonly TaskAppDbContext _dbContext;
    
    public DocumentService(TaskAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Document>> GetDocumentsByTaskIdAsync(int taskId)
    {
        return await _dbContext.Documents
            .Where(c => c.TaskItemId == taskId)
            .Include(d => d.UploadedBy)
            .OrderByDescending(d => d.UploadedAt)
            .ToListAsync();
    }

    public async Task<Document> UploadDocumentAsync(Document document)
    {
        document.UploadedAt = DateTime.Now;
        
        _dbContext.Documents.Add(document);
        await _dbContext.SaveChangesAsync();
        return document;
    }

    public async Task<Document?> GetDocumentByIdAsync(int id)
    {
        return await _dbContext.Documents
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteDocumentAsync(int id)
    {
        var item = await _dbContext.Documents.FindAsync(id);

        if (item != null)
        {
            _dbContext.Documents.Remove(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}