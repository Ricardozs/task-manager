using FluentValidation;
using TaskManager.Application.Common.Validation;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Application.Tasks.Dtos;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tasks;

public class CreateTaskHandler(
    ITaskRepository taskRepository,
    IValidator<CreateTaskCommand> validator)
{
    public async Task<TaskDto> HandleAsync(
        CreateTaskCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ThrowIfInvalidAsync(command, cancellationToken);

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

    internal static TaskDto ToDto(TaskItem task) =>
        new(task.Id, task.Title, task.Description, task.Status, task.DueDate, task.CreatedAt, task.UpdatedAt);
}
