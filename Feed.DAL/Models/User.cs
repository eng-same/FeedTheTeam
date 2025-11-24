using Microsoft.AspNetCore.Identity;


namespace Feed.Domain.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; }= null!;

    public string? ProfilePic { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }= true;

    //Navigation Properties
    public ICollection<Pool> PoolsCreated { get; set; } = new List<Pool>();
}
