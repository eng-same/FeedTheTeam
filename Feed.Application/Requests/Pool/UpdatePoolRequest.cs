using Feed.Application.Requests.PoolOption;

namespace Feed.Application.Requests.Pool;

public class UpdatePoolRequest
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? ClosesAt { get; set; }
    public int Status { get; set; }
    public List<UpdatePoolOptionDto> Options { get; set; } = new();
}
