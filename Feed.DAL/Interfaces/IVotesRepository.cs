namespace Feed.Domain.Interfaces;

public interface IVotesRepository
{
    Task<IEnumerable<Vote>> GetAllVotesAsync(); 
    Task<Vote> GetVoteByIdAsync(int voteId);
    Task AddVoteAsync(Vote vote);
    Task UpdateVoteAsync(Vote vote);
    Task DeleteVoteAsync(int voteId);
}
