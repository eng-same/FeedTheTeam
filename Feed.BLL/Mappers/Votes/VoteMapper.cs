
using Feed.Application.DTOs.Vote;
using Feed.Domain.Models;

namespace Feed.Application.Mappers.Votes;

public static class VoteMapper
{
      public static VoteSummaryDto ToSummaryDto(this IEnumerable<Vote> votes)
    {
        return new VoteSummaryDto
        {
            TotalVotes = votes.Count()
        };
    }
}
