using Feed.Domain.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Feed.API.Helpers;

public static class MigrationHelper
{
    public static void ApplyMigrations(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            var pendingMigrations = db.Database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                Console.WriteLine(" Applying database migrations...");
                db.Database.Migrate();
                Console.WriteLine(" Migrations applied.");
            }
            else
            {
                Console.WriteLine(" No migrations to apply.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Migration error: {ex.Message}");
            throw; // remove if you want API to continue even if migrations fail
        }
    }
}
