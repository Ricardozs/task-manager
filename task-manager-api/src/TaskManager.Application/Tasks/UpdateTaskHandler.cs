using FluentValidation;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Validation;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Application.Tasks.Dtos;
using TaskManager.Application.Tasks.Validators;

namespace TaskManager.Application.Tasks;

public class UpdateTaskHandler(
    ITaskRepository taskRepository,
    IValidator<UpdateTaskValidationContext> validator)
{
    public async Task<TaskDto> HandleAsync(
        UpdateTaskCommand command,
        CancellationToken cancellationToken = default)
    {
        var task = await taskRepository.GetByIdAsync(command.TaskId, cancellationToken);
        if (task is null)
            throw new NotFoundException("Task not found.");

        if (task.UserId != command.UserId)
            throw new ForbiddenException("You do not have access to this task.");

        await validator.ThrowIfInvalidAsync(
            new UpdateTaskValidationContext(command, task),
            cancellationToken);

        task.Update(
            command.Title,
            command.Description,
            command.Status,
            command.DueDate,
            DateTime.UtcNow);

        await taskRepository.UpdateAsync(task, cancellationToken);

        return CreateTaskHandler.ToDto(task);
    }
}
