using FluentAssertions;
using NSubstitute;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Tasks;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tests.Tasks;

public class GetTaskHandlerTests
{
    private readonly ITaskRepository _taskRepository = Substitute.For<ITaskRepository>();
    private readonly GetTaskHandler _sut;

    public GetTaskHandlerTests()
    {
        _sut = new GetTaskHandler(_taskRepository);
    }

    [Fact]
    public async Task HandleAsync_throws_forbidden_when_task_belongs_to_another_user()
    {
        var ownerId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var taskId = Guid.NewGuid();

        var task = TaskItem.Create(
            ownerId,
            "Owner task",
            null,
            TaskStatus.Pending,
            DateOnly.FromDateTime(DateTime.UtcNow),
            DateTime.UtcNow);

        _taskRepository.GetByIdAsync(taskId).Returns(task);

        var query = new GetTaskQuery(otherUserId, taskId);
        var act = () => _sut.HandleAsync(query);

        await act.Should().ThrowAsync<ForbiddenException>()
            .WithMessage("*do not have access*");
    }

    [Fact]
    public async Task HandleAsync_returns_task_when_user_is_owner()
    {
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();

        var task = TaskItem.Create(
            userId,
            "My task",
            "Details",
            TaskStatus.InProgress,
            DateOnly.FromDateTime(DateTime.UtcNow).AddDays(3),
            DateTime.UtcNow);

        _taskRepository.GetByIdAsync(taskId).Returns(task);

        var result = await _sut.HandleAsync(new GetTaskQuery(userId, taskId));

        result.Title.Should().Be("My task");
        result.Status.Should().Be(TaskStatus.InProgress);
    }
}
