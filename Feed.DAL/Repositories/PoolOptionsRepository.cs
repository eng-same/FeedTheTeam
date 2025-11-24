
using Feed.Domain.Data;
using Feed.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Feed.Domain.Repositories;

public class PoolOptionsRepository :IPoolOptionsRepository
{
    private readonly ApplicationDbContext _context;

    public PoolOptionsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PoolOption>> GetAllPoolOptionsAsync()
    {
        return await _context.PoolOptions
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<PoolOption> GetPoolOptionByIdAsync(int poolOptionId)
    {
        return await _context.PoolOptions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == poolOptionId);
    }

    public async Task AddPoolOptionAsync(PoolOption poolOption)
    {
        await _context.PoolOptions.AddAsync(poolOption);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePoolOptionAsync(PoolOption poolOption)
    {
        _context.PoolOptions.Update(poolOption);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePoolOptionAsync(int poolOptionId)
    {
        var option = await _context.PoolOptions.FindAsync(poolOptionId);

        if (option != null)
        {
            _context.PoolOptions.Remove(option);
            await _context.SaveChangesAsync();
        }

    }
}
