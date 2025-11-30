using Feed.Application.Requests.PoolOption;

namespace Feed.Application.Interfaces;

public interface IPoolOptionService
{
    Task<IEnumerable<PoolOptionDto>> GetOptionsByPoolAsync(int poolId);
    Task<PoolOptionDto?> GetOptionByIdAsync(int optionId);

    Task<int> AddOptionAsync(CreatePoolOptionRequest dto, int poolId, string currentUserId);
    Task UpdateOptionAsync(UpdatePoolOptionDto dto, string currentUserId);
    Task DeleteOptionAsync(int optionId, string currentUserId);
}