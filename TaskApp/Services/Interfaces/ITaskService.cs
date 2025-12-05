using TaskApp.Models;

namespace TaskApp.Services.Interfaces;

public interface ITaskService
{
    Task<List<TaskItem>> GetAllTasksAsync();
    Task<TaskItem?> GetTaskByIdAsync(int id);
    Task<TaskItem> CreateTaskAsync(TaskItem task);
    Task UpdateTaskAsync(TaskItem task);
    Task DeleteTaskAsync(int id);
    
    Task<List<TaskItem>> GetTasksByCreatorAsync(string creatorId);
    Task<List<TaskItem>> GetTasksByAssignedAsync(string assignedId);
    Task<List<TaskItem>> GetUnresolvedTasksAsync();
    Task<List<TaskItem>> GetOverdueTasksAsync();
    Task<List<TaskItem>> GetTasksWithOverdueChecklistItemsAsync();
}