using Microsoft.EntityFrameworkCore;
using TaskApp.Data;
using TaskApp.Enums;
using TaskApp.Models;
using TaskApp.Services.Interfaces;

namespace TaskApp.Services;

public class TaskService : ITaskService
{

    private readonly TaskAppDbContext _dbContext;

    public TaskService(TaskAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<TaskItem>> GetAllTasksAsync()
    {
        return await _dbContext.Tasks
            .Include(t => t.Company)
            .Include(t => t.Creator)
            .Include(t => t.Assigned)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        return await _dbContext.Tasks
            .Include(t => t.Company)
            .Include(t => t.Creator)
            .Include(t => t.Assigned)
            .Include(t => t.Comments)
                .ThenInclude(c => c.Author)
            .Include(t => t.CheckListItems.OrderBy(c => c.Order))
            .Include(t => t.Documents)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem task)
    {
        task.CreatedDate = DateTime.Now;
        
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task UpdateTaskAsync(TaskItem task)
    {
        _dbContext.Tasks.Update(task);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await _dbContext.Tasks.FindAsync(id);

        if (task != null)
        {
            _dbContext.Remove(task);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<TaskItem>> GetTasksByCreatorAsync(string creatorId)
    {
        return await _dbContext.Tasks
            .Include(t => t.Company)
            .Include(t => t.Creator)
            .Include(t => t.Assigned)
            .Where(t => t.CreatorId == creatorId)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<List<TaskItem>> GetTasksByAssignedAsync(string assignedId)
    {
        return await _dbContext.Tasks
            .Include(t => t.Company)
            .Include(t => t.Creator)
            .Include(t => t.Assigned)
            .Where(t => t.AssignedId == assignedId)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<List<TaskItem>> GetUnresolvedTasksAsync()
    {
        return await _dbContext.Tasks
            .Include(t => t.Company)
            .Include(t => t.Creator)
            .Include(t => t.Assigned)
            .Where(t => t.Status != TaskItemStatus.Finished && t.Status != TaskItemStatus.Cancelled)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<List<TaskItem>> GetOverdueTasksAsync()
    {
        return await _dbContext.Tasks
            .Include(t => t.Company)
            .Include(t => t.Creator)
            .Include(t => t.Assigned)
            .Where(t => t.DueDate < DateTime.Now && t.Status != TaskItemStatus.Finished && t.Status != TaskItemStatus.Cancelled)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<List<TaskItem>> GetTasksWithOverdueChecklistItemsAsync()
    {
        return await _dbContext.Tasks
            .Include(t => t.Company)
            .Include(t => t.Creator)
            .Include(t => t.Assigned)
            .Include(t => t.CheckListItems)
            .Where(t => t.Status != TaskItemStatus.Finished 
                        && t.Status != TaskItemStatus.Cancelled 
                        && t.CheckListItems.Any(c => c.DueDate < DateTime.Now && !c.IsChecked))
            .OrderByDescending(t => t.Priority)
            .ToListAsync();
    }
}