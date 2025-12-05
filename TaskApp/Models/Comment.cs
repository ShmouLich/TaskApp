namespace TaskApp.Models;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    
    public int TaskItemId { get; set; }
    public string AuthorId { get; set; } = string.Empty;

    public TaskItem TaskItem { get; set; } = null!;
    public User Author { get; set; } = null!;
}