using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public DateOnly DueDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private TaskItem() { }

    public static TaskItem Create(
        Guid userId,
        string title,
        string? description,
        TaskStatus status,
        DateOnly dueDate,
        DateTime utcNow)
    {
        return new TaskItem
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title.Trim(),
            Description = description?.Trim(),
            Status = status,
            DueDate = dueDate,
            CreatedAt = utcNow,
            UpdatedAt = utcNow
        };
    }

    public void Update(
        string title,
        string? description,
        TaskStatus status,
        DateOnly dueDate,
        DateTime utcNow)
    {
        Title = title.Trim();
        Description = description?.Trim();
        Status = status;
        DueDate = dueDate;
        UpdatedAt = utcNow;
    }
}
