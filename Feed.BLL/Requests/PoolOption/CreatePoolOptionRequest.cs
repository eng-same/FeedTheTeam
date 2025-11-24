
namespace Feed.Application.Requests.PoolOption;

public class CreatePoolOptionRequest
{
    public string Name { get; set; } = string.Empty;
    public string OptionText { get; set; } = string.Empty;

    // future: ImageUrl, Location, DeliveryLink, etc.
    //public string? ImageUrl { get; set; }
}
