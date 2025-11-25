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
            Status = dto.Status ?? 1,
            CreatedById = currentUserId
        };

        // save pool first to generate Id
        await _poolRepo.AddPoolAsync(pool);

        // add options
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
        var pool = await _poolRepo.GetPoolByIdAsync(dto.Id);
        if (pool == null) throw new KeyNotFoundException("Pool not found");
        if (pool.CreatedById != currentUserId) throw new UnauthorizedAccessException("Not owner");

        // Update pool simple fields
        pool.Title = dto.Title;
        pool.Description = dto.Description;
        pool.ClosesAt = dto.ClosesAt;
        pool.Status = dto.Status;

        await _poolRepo.UpdatePoolAsync(pool);

        // Manage options: add/update/delete
        var existingOptionIds = pool.Options.Select(o => o.Id).ToList();
        var incomingOptionIds = dto.Options.Where(o => o.Id > 0).Select(o => o.Id).ToList();

        // Delete options that are in DB but not in request
        var toDelete = existingOptionIds.Except(incomingOptionIds).ToList();
        foreach (var id in toDelete)
        {
            await _optionsRepo.DeletePoolOptionAsync(id);
        }

        // Update existing and add new
        foreach (var opt in dto.Options)
        {
            if (opt.Id == 0)
            {
                var newOpt = new PoolOption
                {
                    Name = opt.Name,
                    OptionText = opt.OptionText,
                    CreatedAt = DateTime.UtcNow,
                    PoolId = pool.Id
                };
                await _optionsRepo.AddPoolOptionAsync(newOpt);
            }
            else
            {
                // update
                var u = new PoolOption
                {
                    Id = opt.Id,
                    Name = opt.Name,
                    OptionText = opt.OptionText,
                    PoolId = pool.Id
                };
                await _optionsRepo.UpdatePoolOptionAsync(u);
            }
        }
    }

    public async Task OpenPoolAsync(int poolId, string currentUserId)
    {
        var pool = await _poolRepo.GetPoolByIdAsync(poolId);
        if (pool == null) throw new KeyNotFoundException("Pool not found");
        if (pool.CreatedById != currentUserId) throw new UnauthorizedAccessException();

        pool.Status = 1; // open
        await _poolRepo.UpdatePoolAsync(pool);
    }

    public async Task ClosePoolAsync(int poolId, string currentUserId)
    {
        var pool = await _poolRepo.GetPoolByIdAsync(poolId);
        if (pool == null) throw new KeyNotFoundException("Pool not found");
        if (pool.CreatedById != currentUserId) throw new UnauthorizedAccessException();

        pool.Status = 2; // closed
        await _poolRepo.UpdatePoolAsync(pool);
    }

    public async Task DeletePoolAsync(int poolId, string currentUserId)
    {
        var pool = await _poolRepo.GetPoolByIdAsync(poolId);
        if (pool == null) return;
        if (pool.CreatedById != currentUserId) throw new UnauthorizedAccessException();

        await _poolRepo.DeletePoolAsync(poolId);
    }
}
