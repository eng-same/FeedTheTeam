
namespace Feed.Application.DTOs.Vote;

public class VoteOptionSummaryDto
{
    public int OptionId { get; set; }
    public string OptionText { get; set; } = string.Empty;
    public int VotesCount { get; set; }
}
