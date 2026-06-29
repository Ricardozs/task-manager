using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Tasks.Commands;

namespace TaskManager.Application.Tasks;

public class DeleteTaskHandler(ITaskRepository taskRepository)
{
    public async Task HandleAsync(
        DeleteTaskCommand command,
        CancellationToken cancellationToken = default)
    {
        var task = await taskRepository.GetByIdAsync(command.TaskId, cancellationToken);
        if (task is null)
            throw new NotFoundException("Task not found.");

        if (task.UserId != command.UserId)
            throw new ForbiddenException("You do not have access to this task.");

        await taskRepository.DeleteAsync(task, cancellationToken);
    }
}
