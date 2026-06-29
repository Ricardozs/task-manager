using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Application.Tasks.Dtos;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tasks;

public class CreateTaskHandler(ITaskRepository taskRepository)
{
    public async Task<TaskDto> HandleAsync(
        CreateTaskCommand command,
        CancellationToken cancellationToken = default)
    {
        Validate(command);

        var task = TaskItem.Create(
            command.UserId,
            command.Title,
            command.Description,
            command.Status,
            command.DueDate,
            DateTime.UtcNow);

        await taskRepository.AddAsync(task, cancellationToken);

        return ToDto(task);
    }

    private static void Validate(CreateTaskCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
            throw new ValidationException("Title is required.");

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (command.DueDate < today)
            throw new ValidationException("Due date cannot be in the past.");
    }

    internal static TaskDto ToDto(TaskItem task) =>
        new(task.Id, task.Title, task.Description, task.Status, task.DueDate, task.CreatedAt, task.UpdatedAt);
}
