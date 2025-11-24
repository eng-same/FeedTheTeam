
namespace Feed.Application.DTOs.Statistics;

public class PoolStatisticsDto
{
    public int PoolId { get; set; }
    public IEnumerable<OptionStat> Options { get; set; }
    public int TotalParticipants { get; set; }
}
