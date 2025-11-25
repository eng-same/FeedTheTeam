using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Feed.Domain.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

        builder.UseNpgsql(
            "Host=localhost;Port=5432;Database=FeedDb;Username=postgres;Password=RonaldoRonaldo_777");

        return new ApplicationDbContext(builder.Options);
    }
}
