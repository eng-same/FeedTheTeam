
namespace Feed.Application.DTOs.Vote;

public class VoteSummaryDto
{
    public int TotalVotes { get; set; }
    public int? WinningOptionId { get; set; }       // null if no votes or tie
    public int? UserChoiceOptionId { get; set; }    // null if not voted or anonymous
    public List<VoteOptionSummaryDto> Options { get; set; } = new();
}

