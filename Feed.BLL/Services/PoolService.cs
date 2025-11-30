using Feed.Application.Interfaces;
using Feed.Application.Mappers.Pool;
using Feed.Application.Requests.Pool;
using Feed.Domain.Common;
using Feed.Domain.Interfaces;

namespace Feed.Application.Services;

public class PoolService : IPoolService
{
    private readonly IPoolRepository _poolRepo;
    private readonly IPoolOptionsRepository _optionsRepo;

    public PoolService(IPoolRepository poolRepo, IPoolOptionsRepository optionsRepo)
    {
        _poolRepo = poolRepo;
        _optionsRepo = optionsRepo;
    }

    public async Task<PagedResult<PoolDto>> GetPoolsAsync(PoolFilter filter)
    {
        var paged = await _poolRepo.GetAllPoolsAsync(filter);
        var dtoItems = paged.Items.Select(p => p.ToDto()).ToList();
        return new PagedResult<PoolDto>(dtoItems, paged.TotalCount, paged.Page, paged.PageSize);
    }

    public async Task<PoolDto?> GetPoolByIdAsync(int poolId)
    {
        var pool = await _poolRepo.GetPoolByIdAsync(poolId);
        return pool?.ToDto();
    }

    public async Task<int> CreatePoolAsync(CreatePoolRequest dto, string currentUserId)
    {
        var pool = new Pool
        {
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            ClosesAt = dto.ClosesAt,
            Status = dto.Status ?? 0,
            CreatedById = currentUserId
        };

        
        await _poolRepo.AddPoolAsync(pool);

        
        foreach (var opt in dto.Options)
        {
            var option = new PoolOption
            {
                Name = opt.Name,
                OptionText = opt.OptionText,
                CreatedAt = DateTime.UtcNow,
                PoolId = pool.Id
            };

            await _optionsRepo.AddPoolOptionAsync(option);
        }

        return pool.Id;
    }

    public async Task UpdatePoolAsync(UpdatePoolRequest dto, string currentUserId)
    {
        // 1. Get pool with tracking
        var pool = await _poolRepo.GetPoolByIdAsync(dto.Id);
        if (pool == null) throw new KeyNotFoundException("Pool not found");
        if (pool.CreatedById != currentUserId) throw new UnauthorizedAccessException("Not owner");

        // 2. Update simple fields
        pool.Title = dto.Title;
        pool.Description = dto.Description;
        pool.ClosesAt = dto.ClosesAt;
        pool.Status = dto.Status;

        // 3. Prepare lookup for existing options
        var existing = pool.Options.ToDictionary(o => o.Id);
        var receivedIds = new HashSet<int>();

        foreach (var opt in dto.Options)
        {
            if (opt.Id.HasValue)
            {
                int idExisting = opt.Id.Value;
                receivedIds.Add(idExisting);

                // UPDATE
                if (existing.TryGetValue(idExisting, out var entity))
                {
                    entity.Name = opt.Name;
                    entity.OptionText = opt.OptionText;
                }
            }
            else
            {
                // INSERT NEW
                pool.Options.Add(new PoolOption
                {
                    Name = opt.Name,
                    OptionText = opt.OptionText,
                    CreatedAt = DateTime.UtcNow,
                    PoolId = pool.Id
                });
            }
        }

        // 4. DELETE missing options
        pool.Options.RemoveAll(o => !receivedIds.Contains(o.Id));

        // 5. Save
        await _poolRepo.UpdatePoolAsync(pool);
    }

    public async Task OpenPoolAsync(int poolId, string currentUserId)
    {
        var pool = await _poolRepo.GetPoolByIdAsync(poolId);
        if (pool == null) throw new KeyNotFoundException("Pool not found");
        if (pool.CreatedById != currentUserId) throw new UnauthorizedAccessException();

        pool.Status = 0; // open
        await _poolRepo.UpdatePoolAsync(pool);
    }

    public async Task ClosePoolAsync(int poolId, string currentUserId)
    {
        var pool = await _poolRepo.GetPoolByIdAsync(poolId);
        if (pool == null) throw new KeyNotFoundException("Pool not found");
        if (pool.CreatedById != currentUserId) throw new UnauthorizedAccessException();

        pool.Status = 1; // closed
        await _poolRepo.UpdatePoolAsync(pool);
    }

    public async Task DeletePoolAsync(int poolId, string currentUserId)
    {
        var pool = await _poolRepo.GetPoolByIdAsync(poolId);
        if (pool == null) throw new KeyNotFoundException("Pool not found") ;
        if (pool.CreatedById != currentUserId) throw new UnauthorizedAccessException();

        await _poolRepo.DeletePoolAsync(poolId);
    }
}
