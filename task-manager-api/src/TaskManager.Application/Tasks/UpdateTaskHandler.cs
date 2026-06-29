using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Application.Tasks.Dtos;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tasks;

public class UpdateTaskHandler(ITaskRepository taskRepository)
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

        Validate(command, task);

        task.Update(
            command.Title,
            command.Description,
            command.Status,
            command.DueDate,
            DateTime.UtcNow);

        await taskRepository.UpdateAsync(task, cancellationToken);

        return CreateTaskHandler.ToDto(task);
    }

    private static void Validate(UpdateTaskCommand command, Domain.Entities.TaskItem task)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
            throw new ValidationException("Title is required.");

        if (task.Status == TaskStatus.Completed && command.Status != TaskStatus.Completed)
            throw new ValidationException("Completed tasks cannot revert to another status.");
    }
}
