namespace TaskManager.Application.Tasks.Commands;

public record GetTaskQuery(Guid UserId, Guid TaskId);
