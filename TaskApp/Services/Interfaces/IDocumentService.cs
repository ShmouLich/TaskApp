using TaskApp.Models;

namespace TaskApp.Services.Interfaces;

public interface IDocumentService
{
    Task<List<Document>> GetDocumentsByTaskIdAsync(int taskId);
    Task<Document> UploadDocumentAsync(Document document);
    Task<Document?> GetDocumentByIdAsync(int id);
    Task DeleteDocumentAsync(int id);
}