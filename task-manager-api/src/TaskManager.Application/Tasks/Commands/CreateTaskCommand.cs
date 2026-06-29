namespace TaskManager.Application.Tasks.Commands;

public record CreateTaskCommand(
    Guid UserId,
    string Title,
    string? Description,
    TaskStatus Status,
    DateOnly DueDate);
