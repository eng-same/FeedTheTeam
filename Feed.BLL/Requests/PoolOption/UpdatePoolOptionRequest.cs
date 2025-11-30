namespace Feed.Application.Requests.PoolOption;

public class UpdatePoolOptionDto
{

    public int? Id { get; set; }     // null  = new option
   
    public string Name { get; set; }
   
    public string OptionText { get; set; }
}
