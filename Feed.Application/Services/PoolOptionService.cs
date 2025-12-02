

using Feed.Application.Interfaces;
using Feed.Application.Requests.PoolOption;
using Feed.Domain.Interfaces;

namespace Feed.Application.Services;

public class PoolOptionService : IPoolOptionService
{
    private readonly IPoolRepository _poolRepo;
    private readonly IPoolOptionsRepository _optionsRepo;

    public PoolOptionService(IPoolRepository poolRepo, IPoolOptionsRepository optionsRepo)
    {
        _poolRepo = poolRepo;
        _optionsRepo = optionsRepo;
    }

    public async Task<IEnumerable<PoolOptionDto>> GetOptionsByPoolAsync(int poolId)
    {
        var pool = await _poolRepo.GetPoolByIdAsync(poolId);
        if (pool == null) return Enumerable.Empty<PoolOptionDto>();

        return pool.Options.Select(o => new PoolOptionDto
        {
            Id = o.Id,
            Name = o.Name,
            OptionText = o.OptionText
        }).ToList();
    }

    public async Task<PoolOptionDto?> GetOptionByIdAsync(int optionId)
    {
        var opt = await _optionsRepo.GetPoolOptionByIdAsync(optionId);
        if (opt == null) return null;
        return new PoolOptionDto
        {
            Id = opt.Id,
            Name = opt.Name,
            OptionText = opt.OptionText
        };
    }

    public async Task<int> AddOptionAsync(CreatePoolOptionRequest dto, int poolId, string currentUserId)
    {
        var pool = await _poolRepo.GetPoolByIdAsync(poolId);
        if (pool == null) throw new KeyNotFoundException("Pool not found");
        if (pool.CreatedById != currentUserId) throw new UnauthorizedAccessException();

        var opt = new PoolOption
        {
            Name = dto.Name,
            OptionText = dto.OptionText,
            CreatedAt = DateTime.UtcNow,
            PoolId = poolId
        };

        await _optionsRepo.AddPoolOptionAsync(opt);
        return opt.Id;
    }

    public async Task UpdateOptionAsync(UpdatePoolOptionDto dto, string currentUserId)
    {
        
        if(dto.Id == null) throw new ArgumentException("Option ID is required");
        var existing = await _optionsRepo.GetPoolOptionByIdAsync(dto.Id.Value);
        if (existing == null) throw new KeyNotFoundException("Option not found");

        var pool = await _poolRepo.GetPoolByIdAsync(existing.PoolId);
        if (pool == null) throw new KeyNotFoundException("Parent pool not found");
        if (pool.CreatedById != currentUserId) throw new UnauthorizedAccessException();

        existing.Name = dto.Name;
        existing.OptionText = dto.OptionText;
        await _optionsRepo.UpdatePoolOptionAsync(existing);
    }

    public async Task DeleteOptionAsync(int optionId, string currentUserId)
    {
        var existing = await _optionsRepo.GetPoolOptionByIdAsync(optionId);
        if (existing == null) return;

        var pool = await _poolRepo.GetPoolByIdAsync(existing.PoolId);
        if (pool == null) throw new KeyNotFoundException("Parent pool not found");
        if (pool.CreatedById != currentUserId) throw new UnauthorizedAccessException();

        await _optionsRepo.DeletePoolOptionAsync(optionId);
    }
}
