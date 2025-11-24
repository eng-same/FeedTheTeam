
using Feed.Domain.Common;
using Feed.Domain.Data;
using Feed.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Feed.Domain.Repositories;

public class PoolRepository :IPoolRepository
{
    private readonly ApplicationDbContext _context;

    public PoolRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pool>> GetAllPoolsAsync()
    {
        
        return await _context.Pools
            .Where(p => !p.IsDeleted)
            .Include(p => p.Options)
            .AsNoTracking()
            .ToListAsync();
    }

    //use this for filtering/paging/sorting)
    public async Task<PagedResult<Pool>> GetAllPoolsAsync(PoolFilter filter)
    {
        if (filter == null) filter = new PoolFilter();

        var query = _context.Pools.AsQueryable();

        // Soft delete filter
        if (!filter.IncludeDeleted)
            query = query.Where(p => !p.IsDeleted);

        // Filtering
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var s = filter.Search.Trim();
            query = query.Where(p =>
                EF.Functions.ILike(p.Title, $"%{s}%") ||
                EF.Functions.ILike(p.Description, $"%{s}%"));
        }

        if (filter.Status.HasValue)
            query = query.Where(p => p.Status == filter.Status.Value);

        if (!string.IsNullOrWhiteSpace(filter.CreatedById))
            query = query.Where(p => p.CreatedById == filter.CreatedById);

        if (filter.CreatedFrom.HasValue)
            query = query.Where(p => p.CreatedAt >= filter.CreatedFrom.Value);

        if (filter.CreatedTo.HasValue)
            query = query.Where(p => p.CreatedAt <= filter.CreatedTo.Value);

        // Includes
        if (filter.IncludeOptions)
            query = query.Include(p => p.Options);

        if (filter.IncludeVotes)
            query = query.Include(p => p.Votes);

        // Total count (before paging)
        var totalCount = await query.LongCountAsync();

        // Sorting
        query = (filter?.SortBy?.ToLower()) switch
        {
            "title" => filter.SortDesc ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title),
            "closes" => filter.SortDesc ? query.OrderByDescending(p => p.ClosesAt) : query.OrderBy(p => p.ClosesAt),
            "status" => filter.SortDesc ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status),
            "created" => filter.SortDesc ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.CreatedAt) // default: newest first
        };

        // Paging (sanitize)
        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 200);

        var items = await query
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Pool>(items, totalCount, page, pageSize);
    }

    public async Task<Pool?> GetPoolByIdAsync(int poolId)
    {
        return await _context.Pools
            .AsNoTracking()
            .Include(p => p.Options) 
            .Include(p => p.Votes)
            .FirstOrDefaultAsync(p => p.Id == poolId && !p.IsDeleted);
    }

    public async Task AddPoolAsync(Pool pool)
    {
        await _context.Pools.AddAsync(pool);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePoolAsync(Pool pool)
    {
        _context.Pools.Update(pool);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePoolAsync(int poolId)
    {
        var pool = await _context.Pools.FindAsync(poolId);

        if (pool == null) return;

        pool.IsDeleted = true;
        _context.Pools.Update(pool);
        await _context.SaveChangesAsync();
    }
}
