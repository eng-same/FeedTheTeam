namespace Feed.Domain.Interfaces;

public interface IVotesRepository
{
    Task<IEnumerable<Vote>> GetAllVotesAsync(int poolId); 
    Task<Vote> GetVoteByIdAsync(int voteId);
    Task AddVoteAsync(Vote vote);
    Task UpdateVoteAsync(Vote vote);
    Task DeleteVoteAsync(int voteId);
    Task<bool> HasUserVotedAsync(int poolId, string userId);
}
