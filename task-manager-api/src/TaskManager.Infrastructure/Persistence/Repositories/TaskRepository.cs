using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class TaskRepository(AppDbContext dbContext) : ITaskRepository
{
    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<IReadOnlyList<TaskItem>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default) =>
        await dbContext.Tasks
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        dbContext.Tasks.Update(task);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        dbContext.Tasks.Remove(task);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
