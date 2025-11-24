
namespace Feed.Application.DTOs.Pool;

public class PoolDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime? ClosesAt { get; set; }
    public int Status { get; set; }

    public string CreatedById { get; set; } = "";
    public IEnumerable<PoolOptionDto> Options { get; set; } = new List<PoolOptionDto>();
}  
