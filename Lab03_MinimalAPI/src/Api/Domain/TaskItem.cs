namespace Lab03_MinimalAPI.Domain;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
