namespace Lab03_MinimalAPI.DTOs;
public record TaskCreateDto(string Title, string? Description, int UserId);
