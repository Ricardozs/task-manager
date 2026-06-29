namespace TaskManager.Application.Tasks.Commands;

public record UpdateTaskCommand(
    Guid UserId,
    Guid TaskId,
    string Title,
    string? Description,
    TaskStatus Status,
    DateOnly DueDate);
