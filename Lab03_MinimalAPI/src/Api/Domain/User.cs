using System.ComponentModel.DataAnnotations;

namespace Lab03_MinimalAPI.Domain;

public class User
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

}
