namespace TaskApp.Models;

public class Document
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    
    public int TaskItemId { get; set; }
    public string UploadedById { get; set; } = string.Empty;

    public TaskItem TaskItem { get; set; } = null!;
    public User UploadedBy { get; set; } = null!;
}