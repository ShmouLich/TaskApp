using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskApp.Enums;
using TaskApp.Models;


namespace TaskApp.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(TaskAppDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (await dbContext.Users.AnyAsync()) {
            return;
        }
        
        // users
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        var users = new List<User>();
        var userFaker = new Faker<User>()
            .RuleFor(u => u.UserName, f => f.Internet.Email())
            .RuleFor(u => u.Email, (f, u) => u.UserName)
            .RuleFor(u => u.Name, f => f.Person.FirstName)
            .RuleFor(u => u.Surname, f => f.Person.LastName);

        for (int i = 0; i < 5; i++)
        {
            var user = userFaker.Generate();
            await userManager.CreateAsync(user, "Password123!");
            users.Add(user);
        }
        
        await dbContext.SaveChangesAsync();
        
        // companies
        var companyFaker = new Faker<Company>()
            .RuleFor(c => c.Name, f => f.Company.CompanyName());
        var companies = companyFaker.Generate(10);
        await dbContext.Companies.AddRangeAsync(companies);
        await dbContext.SaveChangesAsync();
        
        // tasks
        var random = new Random();
        var taskFaker = new Faker<TaskItem>()
            .RuleFor(t => t.Description, f => f.Lorem.Sentence(10))
            .RuleFor(t => t.CompanyId, f => f.PickRandom(companies).Id)
            .RuleFor(t => t.AssignedId, f => f.PickRandom(users).Id)
            .RuleFor(t => t.CreatorId, f => f.PickRandom(users).Id)
            .RuleFor(t => t.CreatedDate, f => f.Date.Past())
            .RuleFor(t => t.DueDate, f => f.Date.Future())
            .RuleFor(t => t.Priority, f => f.PickRandom<TaskPriority>())
            .RuleFor(t => t.Status, f => f.PickRandom<TaskItemStatus>())
            .RuleFor(t => t.CompletedDate,
                (f, t) => t.Status == TaskItemStatus.Finished || t.Status == TaskItemStatus.Cancelled
                    ? f.Date.Between(t.CreatedDate, DateTime.Now)
                    : null);
    
        var tasks = taskFaker.Generate(30); 
        await dbContext.Tasks.AddRangeAsync(tasks);
        await dbContext.SaveChangesAsync();
        
        // comments
        var commentFaker = new Faker<Comment>()
            .RuleFor(c => c.Text, f => f.Lorem.Sentence(10))
            .RuleFor(c => c.AuthorId, f => f.PickRandom(users).Id);
        
        var comments = new List<Comment>();
        foreach (var task in tasks)
        {
            var taskComments = commentFaker
                .RuleFor(c => c.TaskItemId, _ => task.Id)
                .RuleFor(c => c.Created, f => f.Date.Between(task.CreatedDate, DateTime.Now))
                .Generate(random.Next(2, 5));
            
            comments.AddRange(taskComments);
        }
        await dbContext.Comments.AddRangeAsync(comments);
        await dbContext.SaveChangesAsync();
        
        // checklist items
        var checkListItemFaker = new Faker<CheckListItem>();
        var checkListItems = new List<CheckListItem>();
        
        foreach (var task in tasks)
        {
            var checkListItem = checkListItemFaker
                .RuleFor(c => c.TaskItemId, _ => task.Id)
                .RuleFor(c => c.Description, f => f.Lorem.Sentence(10))
                .RuleFor(c => c.IsChecked, f => f.Random.Bool())
                .RuleFor(c => c.Order, f => f.IndexFaker)
                .RuleFor(c => c.DueDate, f => f.Date.Between(task.CreatedDate, task.DueDate ?? DateTime.Now.AddMonths(1)))
                .RuleFor(c => c.CompletedDate, (f, c) => c.IsChecked ? f.Date.Between(task.CreatedDate, DateTime.Now) : null)
                .Generate(random.Next(2, 7));
            
            checkListItems.AddRange(checkListItem);
        }
        
        await dbContext.CheckListItems.AddRangeAsync(checkListItems);
        await dbContext.SaveChangesAsync();
    }
    
}