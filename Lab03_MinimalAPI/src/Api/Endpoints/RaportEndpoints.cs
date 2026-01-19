using Lab03_MinimalAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab03_MinimalAPI.Endpoints;

public static class ReportEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/v1/reports");

        group.MapGet("/new-users", async (DateTime from, DateTime to, AppDbContext db) =>
        {
            var users = await db.Users
                .Where(u => u.Id > 0) // w InMemory brak daty rejestracji — uproszczone
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(new { count = users.Count, from, to });
        });
    }
}
