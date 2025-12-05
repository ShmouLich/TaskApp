using TaskApp.Models;

namespace TaskApp.Services.Interfaces;

public interface ICheckListService
{
    Task<List<CheckListItem>> GetCheckListItemsByTaskIdAsync(int taskId);
    Task<CheckListItem> AddCheckListItemAsync(CheckListItem checkListItem);
    Task UpdateCheckListItemAsync(CheckListItem checkListItem);
    Task DeleteCheckListItemAsync(int id);
    Task ToggleCheckListItemAsync(int id);
}