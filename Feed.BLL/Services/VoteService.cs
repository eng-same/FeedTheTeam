

using Feed.Application.Interfaces;
using Feed.Domain.Interfaces;

namespace Feed.Application.Services;

public class VoteService :IVoteService
{
    private readonly IVotesRepository _votesRepo;
    private readonly IPoolRepository _poolRepo;
    private readonly IPoolOptionsRepository _optionsRepo;

    public VoteService(IVotesRepository votesRepo, IPoolRepository poolRepo, IPoolOptionsRepository optionsRepo)
    {
        _votesRepo = votesRepo;
        _poolRepo = poolRepo;
        _optionsRepo = optionsRepo;
    }

    public async Task<IEnumerable<Vote>> GetVotesByPoolAsync(int poolId)
    {
        var all = await _votesRepo.GetAllVotesAsync();
        return all.Where(v => v.PoolId == poolId).ToList();
    }

    public async Task<Vote?> GetVoteByIdAsync(int voteId)
    {
        return await _votesRepo.GetVoteByIdAsync(voteId);
    }

    public async Task<Vote> AddVoteAsync(int poolId, int poolOptionId, string userId)
    {
        var pool = await _poolRepo.GetPoolByIdAsync(poolId);
        if (pool == null) throw new KeyNotFoundException("Pool not found");

        // Example rule: cannot vote if pool is closed (status 2)
        if (pool.Status == 2) throw new InvalidOperationException("Pool is closed");

        var option = await _optionsRepo.GetPoolOptionByIdAsync(poolOptionId);
        if (option == null || option.PoolId != poolId) throw new KeyNotFoundException("Option not found");

        // Simple check: use current repositories (could be optimized)
        var allVotes = await _votesRepo.GetAllVotesAsync();
        if (allVotes.Any(v => v.PoolId == poolId && v.UserId == userId))
            throw new InvalidOperationException("User already voted in this pool");

        var vote = new Vote
        {
            PoolId = poolId,
            PoolOptionId = poolOptionId,
            UserId = userId,
            VotedAt = DateTime.UtcNow
        };

        await _votesRepo.AddVoteAsync(vote);
        return vote;
    }

    public async Task RemoveVoteAsync(int voteId, string currentUserId)
    {
        var vote = await _votesRepo.GetVoteByIdAsync(voteId);
        if (vote == null) return;

        if (vote.UserId != currentUserId) throw new UnauthorizedAccessException();

        await _votesRepo.DeleteVoteAsync(voteId);
    }

    public async Task<bool> HasUserVotedAsync(int poolId, string userId)
    {
        var all = await _votesRepo.GetAllVotesAsync();
        return all.Any(v => v.PoolId == poolId && v.UserId == userId);
    }
}