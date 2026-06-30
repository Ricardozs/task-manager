using FluentAssertions;
using NSubstitute;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Tasks;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Application.Tasks.Validators;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tests.Tasks;

public class UpdateTaskHandlerTests
{
    private readonly ITaskRepository _taskRepository = Substitute.For<ITaskRepository>();
    private readonly UpdateTaskHandler _sut;

    public UpdateTaskHandlerTests()
    {
        _sut = new UpdateTaskHandler(_taskRepository, new UpdateTaskValidationContextValidator());
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

        var command = new UpdateTaskCommand(
            otherUserId,
            taskId,
            "Updated",
            null,
            TaskStatus.Pending,
            DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));

        var act = () => _sut.HandleAsync(command);

        await act.Should().ThrowAsync<ForbiddenException>();
        await _taskRepository.DidNotReceive().UpdateAsync(Arg.Any<TaskItem>());
    }

    [Fact]
    public async Task HandleAsync_allows_past_due_date_on_update()
    {
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var pastDueDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-5);

        var task = TaskItem.Create(
            userId,
            "Existing task",
            null,
            TaskStatus.Pending,
            pastDueDate,
            DateTime.UtcNow.AddDays(-10));

        _taskRepository.GetByIdAsync(taskId).Returns(task);

        var command = new UpdateTaskCommand(
            userId,
            taskId,
            "Updated title",
            null,
            TaskStatus.InProgress,
            pastDueDate);

        var result = await _sut.HandleAsync(command);

        result.Title.Should().Be("Updated title");
        result.Status.Should().Be(TaskStatus.InProgress);
        result.DueDate.Should().Be(pastDueDate);
        await _taskRepository.Received(1).UpdateAsync(task, Arg.Any<CancellationToken>());
    }
}
