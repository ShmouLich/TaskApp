using System.ComponentModel.DataAnnotations;
using TaskApp.Enums;

namespace TaskApp.Models;

public class TaskItem
{
    public int Id { get; set; }
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public int CompanyId { get; set; }
    public string AssignedId { get; set; } = string.Empty;
    public string CreatorId { get; set; } = string.Empty;
    
    public DateTime CreatedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    
    public TaskItemStatus Status { get; set; }
    public TaskPriority Priority { get; set; }

    // navigation properties
    public Company Company { get; set; } = null!;
    public User Assigned { get; set; } = null!;
    public User Creator { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
    public ICollection<CheckListItem> CheckListItems { get; set; } = new List<CheckListItem>();
    

}