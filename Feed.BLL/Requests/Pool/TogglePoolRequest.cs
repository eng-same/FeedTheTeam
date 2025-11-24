
namespace Feed.Application.Requests.Pool;

public class TogglePoolRequest
{
    public int PoolId { get; set; }
    public string UserId { get; set; } = string.Empty;
}
