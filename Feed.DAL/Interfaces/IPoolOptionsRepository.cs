namespace Feed.Domain.Interfaces;

public interface IPoolOptionsRepository
{
    Task<IEnumerable<PoolOption>> GetAllPoolOptionsAsync(); 
    Task<IEnumerable<PoolOption>> GetAllPoolOptionsAsync(int poolOptionId); 
    Task<PoolOption> GetPoolOptionByIdAsync(int poolOptionId);
    Task AddPoolOptionAsync(PoolOption poolOption);
    Task UpdatePoolOptionAsync(PoolOption poolOption);
    Task DeletePoolOptionAsync(int poolOptionId);
}
