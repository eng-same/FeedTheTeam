namespace Feed.Domain.Interfaces;

public  interface IPoolRepository
{
    Task<IEnumerable<Pool>> GetAllPoolsAsync(); //with filtering, paging, sorting
    Task<Pool> GetPoolByIdAsync(int poolId);
    Task AddPoolAsync(Pool pool);
    Task UpdatePoolAsync(Pool pool);
    Task DeletePoolAsync(int poolId); //soft delete
}
