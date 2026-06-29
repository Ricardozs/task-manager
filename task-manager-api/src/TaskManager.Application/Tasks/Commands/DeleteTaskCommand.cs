namespace TaskManager.Application.Tasks.Commands;

public record DeleteTaskCommand(Guid UserId, Guid TaskId);
