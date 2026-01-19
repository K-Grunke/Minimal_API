using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Lab03_MinimalAPI.Data;
using Lab03_MinimalAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Lab03_MinimalAPI.Endpoints;

public static class UserEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/v1/users");

        group.MapPost("/register", async (UserRegisterDto dto, AppDbContext db, IMapper mapper) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Password))
                return Results.BadRequest(new { message = "Password is required" });

            var exists = await db.Users.AnyAsync(u => u.Username == dto.Username);
            if (exists)
                return Results.BadRequest(new { message = "Username already taken" });

            var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(dto.Password)));
            var user = new Domain.User { Username = dto.Username, Email = dto.Email, PasswordHash = hash };
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Created($"/api/v1/users/{user.Id}", mapper.Map<UserDto>(user));
        });

        group.MapPost("/login", async (UserLoginDto dto, AppDbContext db, IConfiguration config) =>
        {
        var user = await db.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
                return Results.Unauthorized();

        var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(dto.Password)));
            if (user.PasswordHash != hash)
                return Results.Unauthorized();

        // pobierz role użytkownika
        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        // dodaj role do tokena
        foreach (var role in roles)
        claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            Environment.GetEnvironmentVariable("JWT_KEY") ??
            config["Jwt:Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Results.Ok(new { token = jwt });
        });

        group.MapGet("/", async (AppDbContext db, IMapper mapper) =>
        {
            var users = await db.Users.AsNoTracking().ToListAsync();
            return Results.Ok(mapper.Map<IEnumerable<UserDto>>(users));
        });

        group.MapGet("/{id:int}", async (int id, AppDbContext db, IMapper mapper) =>
        {
            var user = await db.Users.FindAsync(id);
            return user is null ? Results.NotFound() : Results.Ok(mapper.Map<UserDto>(user));
        });

        group.MapPut("/{id:int}", async (int id, UserRegisterDto dto, AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);
            if (user == null) return Results.NotFound();

            user.Username = dto.Username;
            user.Email = dto.Email;
            user.PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(dto.Password)));
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);
            if (user == null) return Results.NotFound();
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}

// plik do rozbudowy/modyfikacji w przyszłości