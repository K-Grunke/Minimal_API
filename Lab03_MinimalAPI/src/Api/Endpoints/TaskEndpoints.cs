using Lab03_MinimalAPI.Data;
using Lab03_MinimalAPI.Domain;
using Lab03_MinimalAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Lab03_MinimalAPI.Endpoints;

public static class TaskEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/v1/tasks").RequireAuthorization();

        group.MapPost("/", async (TaskCreateDto dto, AppDbContext db) =>
        {
            if (!await db.Users.AnyAsync(u => u.Id == dto.UserId))
                return Results.BadRequest(new { message = "User not found" });

            var task = new TaskItem { Title = dto.Title, Description = dto.Description, UserId = dto.UserId };
            db.Tasks.Add(task);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/tasks/{task.Id}", task);
        });

        group.MapGet("/users/{id:int}", async (int id, AppDbContext db) =>
        {
            var tasks = await db.Tasks.Where(t => t.UserId == id).ToListAsync();
            return Results.Ok(tasks);
        });
    }
}

// plik do rozbudowy/modyfikacji w przyszłości