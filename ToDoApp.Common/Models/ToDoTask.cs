namespace ToDoApp.Common.Models;

public class ToDoTask
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpiryAt { get; set; }
    public int? PercentCompleted { get; set; }
    public bool IsCompleted { get; set; }
}
