using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Application.Tasks.Dtos;

namespace TaskManager.Application.Tasks;

public class ListTasksHandler(ITaskRepository taskRepository)
{
    public async Task<IReadOnlyList<TaskDto>> HandleAsync(
        ListTasksQuery query,
        CancellationToken cancellationToken = default)
    {
        var tasks = await taskRepository.GetByUserIdAsync(query.UserId, cancellationToken);
        return tasks.Select(CreateTaskHandler.ToDto).ToList();
    }
}
