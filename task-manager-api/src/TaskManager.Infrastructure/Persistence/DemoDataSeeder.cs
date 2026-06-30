using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Infrastructure.Persistence;

public static class DemoDataSeeder
{
    public const string DemoEmail = "demo@example.com";
    public const string DemoPassword = "Demo123!";
    public const int DemoTaskCount = 3;

    public static async Task<bool> SeedAsync(
        AppDbContext db,
        IPasswordHasher passwordHasher,
        CancellationToken cancellationToken = default)
    {
        if (await db.Users.AnyAsync(cancellationToken))
        {
            return false;
        }

        var now = DateTime.UtcNow;
        var user = User.Create(DemoEmail, passwordHasher.Hash(DemoPassword), now);
        db.Users.Add(user);

        db.Tasks.AddRange(
            TaskItem.Create(
                user.Id,
                "Review project plan",
                "Read the implementation plan and mark completed phases.",
                TaskStatus.Completed,
                DateOnly.FromDateTime(now.AddDays(-1)),
                now),
            TaskItem.Create(
                user.Id,
                "Configure CORS",
                "Allow the Angular dev server to call the API.",
                TaskStatus.InProgress,
                DateOnly.FromDateTime(now.AddDays(3)),
                now),
            TaskItem.Create(
                user.Id,
                "Write README",
                "Document how-to-run instructions for API and app.",
                TaskStatus.Pending,
                DateOnly.FromDateTime(now.AddDays(7)),
                now));

        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
