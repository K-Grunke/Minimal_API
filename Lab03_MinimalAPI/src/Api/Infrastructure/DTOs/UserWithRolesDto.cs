namespace Lab03_MinimalAPI.DTOs;

public record UserWithRolesDto(int Id, string Username, string Email, IEnumerable<RoleDto> Roles);
