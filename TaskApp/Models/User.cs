using Microsoft.AspNetCore.Identity;

namespace TaskApp.Models;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    
    public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
    public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
    
}