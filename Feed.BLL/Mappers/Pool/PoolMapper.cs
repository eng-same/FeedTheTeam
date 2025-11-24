using PoolModel = Feed.Domain.Models.Pool;
namespace Feed.Application.Mappers.Pool;

public static class PoolMapper
{
    public static PoolDto ToDto(this PoolModel entity)
    {
        return new PoolDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            CreatedAt = entity.CreatedAt,
            ClosesAt = entity.ClosesAt,
            Status = entity.Status,
            CreatedById = entity.CreatedById,
            Options = entity.Options.Select(o => new PoolOptionDto
            {
                Id = o.Id,
                Name = o.Name,
                OptionText = o.OptionText
            })
        };
    }
}
