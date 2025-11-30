
using Feed.Domain.Data;
using Feed.Domain.Interfaces;
using Feed.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Feed.Domain.Repositories;

public class VotesRepository :IVotesRepository
{
    private readonly ApplicationDbContext _context;

    public VotesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vote>> GetAllVotesAsync(int poolId)
    {
        return await _context.Votes
       .AsNoTracking()
       .Where(v => v.PoolId == poolId)
       .Include(v => v.Pool)         // include only if you need navigation data
       .Include(v => v.PoolOption)
       .Include(v => v.User)
       .ToListAsync();
    }

    public async Task<Vote?> GetVoteByIdAsync(int voteId)
    {
        return await _context.Votes
            .AsNoTracking()
            .Include(v => v.PoolOption)
            .Include(v => v.User)
            .FirstOrDefaultAsync(v => v.Id == voteId);
    }

    public async Task AddVoteAsync(Vote vote)
    {
        await _context.Votes.AddAsync(vote);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateVoteAsync(Vote vote)
    {
        _context.Votes.Update(vote);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteVoteAsync(int voteId)
    {
        var vote = await _context.Votes.FindAsync(voteId);

        if (vote != null)
        {
            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasUserVotedAsync(int poolId, string userId)
    {
        return await _context.Votes
           .AsNoTracking()
           .AnyAsync(v => v.PoolId == poolId && v.UserId == userId);
    }
}
