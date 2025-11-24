namespace Feed.Application.Requests.PoolOption;

public class UpdatePoolOptionRequest
{

    public int Id { get; set; } // 0 => new
    public string Name { get; set; } = string.Empty;
    public string OptionText { get; set; } = string.Empty;
}
