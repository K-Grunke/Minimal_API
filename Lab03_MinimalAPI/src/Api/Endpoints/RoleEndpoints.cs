using Lab03_MinimalAPI.Data;
using Lab03_MinimalAPI.Domain;
using Lab03_MinimalAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Lab03_MinimalAPI.Endpoints;

public static class RoleEndpoints
{
    public static void Map(WebApplication app)
    {
        // Grupa endpointów ról
        var group = app.MapGroup("/api/v1/roles")
            .RequireAuthorization(); // wymaga JWT ogólnie

        // 📌 1. Utworzenie nowej roli (tylko ADMIN)
        group.MapPost("/", [Authorize(Roles = "Admin")] async (AppDbContext db, RoleCreateDto dto) =>
        {
            var exists = await db.Roles.AnyAsync(r => r.Name == dto.Name);
            if (exists)
                return Results.BadRequest(new { message = "Role already exists" });

            var role = new Role { Name = dto.Name };
            db.Roles.Add(role);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/roles/{role.Id}", role);
        });

        // 📌 2. Pobranie wszystkich ról (Admin i Manager)
        group.MapGet("/", [Authorize(Roles = "Admin,Manager")] async (AppDbContext db) =>
        {
            var roles = await db.Roles.AsNoTracking().ToListAsync();
            return Results.Ok(roles);
        });

        // 📌 3. Przypisanie roli użytkownikowi (tylko ADMIN)
        group.MapPost("/assign", [Authorize(Roles = "Admin")] async (AppDbContext db, UserRoleAssignDto dto) =>
        {
            var userExists = await db.Users.AnyAsync(u => u.Id == dto.UserId);
            var roleExists = await db.Roles.AnyAsync(r => r.Id == dto.RoleId);

            if (!userExists || !roleExists)
                return Results.BadRequest(new { message = "Invalid user or role" });

            var alreadyAssigned = await db.UserRoles
                .AnyAsync(x => x.UserId == dto.UserId && x.RoleId == dto.RoleId);

            if (alreadyAssigned)
                return Results.BadRequest(new { message = "User already has this role" });

            var ur = new UserRole { UserId = dto.UserId, RoleId = dto.RoleId };
            db.UserRoles.Add(ur);
            await db.SaveChangesAsync();
            return Results.Created("/api/v1/roles/assign", ur);
        });


        // 📌 4. Pobranie wszystkich przypisań użytkowników do ról (tylko ADMIN)
        group.MapGet("/assignments", [Authorize(Roles = "Admin")] async (AppDbContext db) =>
        {
            var assignments = await db.UserRoles
                .Include(ur => ur.User)
                .Include(ur => ur.Role)
                .Select(ur => new
                {
                    User = ur.User.Username,
                    Role = ur.Role.Name
                })
                .ToListAsync();

            return Results.Ok(assignments);
        });

        // 📌 5. Usunięcie przypisania roli użytkownikowi (tylko ADMIN)
        group.MapDelete("/assign/{userId:int}/{roleId:int}", [Authorize(Roles = "Admin")] async (int userId, int roleId, AppDbContext db) =>
        {
            var ur = await db.UserRoles
                .FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);

            if (ur == null)
                return Results.NotFound(new { message = "Assignment not found" });

            db.UserRoles.Remove(ur);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // 📌 6. Usunięcie roli (tylko ADMIN)
        group.MapDelete("/{id:int}", [Authorize(Roles = "Admin")] async (int id, AppDbContext db) =>
        {
            var role = await db.Roles.FindAsync(id);
            if (role == null)
                return Results.NotFound(new { message = "Role not found" });

            db.Roles.Remove(role);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
