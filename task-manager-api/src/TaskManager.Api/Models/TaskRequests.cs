namespace TaskManager.Api.Models;

public record CreateTaskRequest(
    string Title,
    string? Description,
    TaskStatus Status,
    DateOnly DueDate);

public record UpdateTaskRequest(
    string Title,
    string? Description,
    TaskStatus Status,
    DateOnly DueDate);
