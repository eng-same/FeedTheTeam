using Feed.Application.Requests.PoolOption;

namespace Feed.Application.Requests.Pool;

public class CreatePoolRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? ClosesAt { get; set; }
    public List<CreatePoolOptionRequest> Options { get; set; } = new();
    public int? Status { get; set; } // optional
}
