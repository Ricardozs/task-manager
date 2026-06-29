namespace TaskManager.Application.Tasks.Dtos;

public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    TaskStatus Status,
    DateOnly DueDate,
    DateTime CreatedAt,
    DateTime UpdatedAt);
