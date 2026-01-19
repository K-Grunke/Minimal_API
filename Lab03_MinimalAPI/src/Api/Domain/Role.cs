namespace Lab03_MinimalAPI.Domain;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // relacja wiele-do-wielu z User
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
