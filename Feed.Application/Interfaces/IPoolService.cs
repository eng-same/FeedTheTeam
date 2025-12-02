using Feed.Application.Requests.Pool;
using Feed.Domain.Common;


namespace Feed.Application.Interfaces;

public interface IPoolService
{
    // Queries
    Task<PagedResult<PoolDto>> GetPoolsAsync(PoolFilter filter);
    Task<PoolDto?> GetPoolByIdAsync(int poolId);

    // Commands
    Task<int> CreatePoolAsync(CreatePoolRequest dto, string currentUserId);
    Task UpdatePoolAsync(UpdatePoolRequest dto, string currentUserId);
    Task OpenPoolAsync(int poolId, string currentUserId);
    Task ClosePoolAsync(int poolId, string currentUserId);
    Task DeletePoolAsync(int poolId, string currentUserId);
}