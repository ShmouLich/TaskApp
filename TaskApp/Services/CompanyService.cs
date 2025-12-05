using Microsoft.EntityFrameworkCore;
using TaskApp.Data;
using TaskApp.Models;
using TaskApp.Services.Interfaces;

namespace TaskApp.Services;

public class CompanyService : ICompanyService
{
    
    private readonly TaskAppDbContext _dbContext;
    
    public CompanyService(TaskAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Company>> GetAllCompaniesAsync()
    {
        return await _dbContext.Companies.ToListAsync();
    }

    public async Task<Company?> GetCompanyByIdAsync(int id)
    {
        return await _dbContext.Companies
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Company> CreateCompanyAsync(Company company)
    {
        _dbContext.Companies.Add(company);
        await _dbContext.SaveChangesAsync();
        
        return company;
    }

    public async Task UpdateCompanyAsync(Company company)
    {
        _dbContext.Companies.Update(company);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCompanyAsync(int id)
    {
        var item = await _dbContext.Companies.FindAsync(id);

        if (item != null)
        {
            _dbContext.Companies.Remove(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}