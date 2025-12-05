namespace TaskApp.Models;

public class CheckListItem
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsChecked { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public int Order { get; set; }
    public int TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; } = null!;
}