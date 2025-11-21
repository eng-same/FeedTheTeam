namespace Feed.Domain.Interfaces;

public interface IPoolOptionsRepository
{
    Task<IEnumerable<PoolOption>> GetAllPoolOptionsAsync(); 
    Task<PoolOption> GetPoolOptionByIdAsync(int poolOptionId);
    Task AddPoolOptionAsync(PoolOption poolOption);
    Task UpdatePoolOptionAsync(PoolOption poolOption);
    Task DeletePoolOptionAsync(int poolOptionId);
}
