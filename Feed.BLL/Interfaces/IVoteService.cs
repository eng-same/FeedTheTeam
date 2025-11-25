namespace Feed.Application.Interfaces;

public interface IVoteService
{
    Task<IEnumerable<Vote>> GetVotesByPoolAsync(int poolId);
    Task<Vote?> GetVoteByIdAsync(int voteId);
    Task<Vote> AddVoteAsync(int poolId, int poolOptionId, string userId);
    Task RemoveVoteAsync(int voteId, string currentUserId);
    Task<bool> HasUserVotedAsync(int poolId, string userId);
}

