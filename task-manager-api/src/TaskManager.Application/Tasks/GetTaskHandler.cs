using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Application.Tasks.Dtos;

namespace TaskManager.Application.Tasks;

public class GetTaskHandler(ITaskRepository taskRepository)
{
    public async Task<TaskDto> HandleAsync(
        GetTaskQuery query,
        CancellationToken cancellationToken = default)
    {
        var task = await taskRepository.GetByIdAsync(query.TaskId, cancellationToken);
        if (task is null)
            throw new NotFoundException("Task not found.");

        if (task.UserId != query.UserId)
            throw new ForbiddenException("You do not have access to this task.");

        return CreateTaskHandler.ToDto(task);
    }
}
