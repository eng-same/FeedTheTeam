
namespace Feed.Domain.Models;

public class Vote
{
    public int Id { get; set; }

    public DateTime VotedAt { get; set; }
    

    //Fks
    public int PoolId { get; set; }

    public int PoolOptionId { get; set; }

    public string UserId { get; set; } = null!;

    //Navigation Properties

    public Pool Pool { get; set; } = null!;

    public PoolOption PoolOption { get; set; } = null!;

    public User User { get; set; } = null!;
}
