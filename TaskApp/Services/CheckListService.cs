using Microsoft.EntityFrameworkCore;
using TaskApp.Data;
using TaskApp.Models;
using TaskApp.Services.Interfaces;

namespace TaskApp.Services;

public class CheckListService : ICheckListService
{
    private readonly TaskAppDbContext _dbContext;

    public CheckListService(TaskAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<CheckListItem>> GetCheckListItemsByTaskIdAsync(int taskId)
    {
        return await _dbContext.CheckListItems
            .Where(c => c.TaskItemId == taskId)
            .OrderBy(c => c.Order)
            .ToListAsync();
    }

    public async Task<CheckListItem> AddCheckListItemAsync(CheckListItem checkListItem)
    {
        if (checkListItem.Order == 0)
        {
            var maxOrder = await _dbContext.CheckListItems
                .Where(c => c.TaskItemId == checkListItem.TaskItemId)
                .MaxAsync(c => (int?) c.Order) ?? 0;
            
            checkListItem.Order = maxOrder + 1;
        }
        
        _dbContext.CheckListItems.Add(checkListItem);
        await _dbContext.SaveChangesAsync();
        
        return checkListItem;
    }

    public async Task UpdateCheckListItemAsync(CheckListItem checkListItem)
    {
        _dbContext.CheckListItems.Update(checkListItem);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCheckListItemAsync(int id)
    {
        var item = await _dbContext.CheckListItems.FindAsync(id);

        if (item != null)
        {
            _dbContext.CheckListItems.Remove(item);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task ToggleCheckListItemAsync(int id)
    {
        var item = await _dbContext.CheckListItems.FindAsync(id);

        if (item != null)
        {
            item.IsChecked = !item.IsChecked;
            item.CompletedDate = item.IsChecked ? DateTime.Now : null;
            await _dbContext.SaveChangesAsync();
        }
    }
}