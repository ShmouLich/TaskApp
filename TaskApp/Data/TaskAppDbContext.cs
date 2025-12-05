using Microsoft.EntityFrameworkCore;
using TaskApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TaskApp.Data;

public class TaskAppDbContext(DbContextOptions<TaskAppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<CheckListItem> CheckListItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // task - company
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Company)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // task - assignee
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Assigned)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssignedId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // task - creator
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Creator)
            .WithMany(u => u.CreatedTasks)
            .HasForeignKey(t => t.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // comment - task
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.TaskItem)
            .WithMany(c => c.Comments)
            .HasForeignKey(t => t.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // comment - author
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Author)
            .WithMany(c => c.Comments)
            .HasForeignKey(t => t.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // document - task
        modelBuilder.Entity<Document>()
            .HasOne(c => c.TaskItem)
            .WithMany(c => c.Documents)
            .HasForeignKey(t => t.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // document - author
        modelBuilder.Entity<Document>()
            .HasOne(d => d.UploadedBy)
            .WithMany(c => c.Documents)
            .HasForeignKey(t => t.UploadedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        // checklist item - task
        modelBuilder.Entity<CheckListItem>()
            .HasOne(c => c.TaskItem)
            .WithMany(c => c.CheckListItems)
            .HasForeignKey(t => t.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);

        // indexy
        modelBuilder.Entity<TaskItem>()
            .HasIndex(t => t.Status);

        modelBuilder.Entity<TaskItem>()
            .HasIndex(t => t.DueDate);

        modelBuilder.Entity<TaskItem>()
            .HasIndex(t => t.AssignedId);

        modelBuilder.Entity<TaskItem>()
            .HasIndex(t => t.CreatorId);

    }
    
}