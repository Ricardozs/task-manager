using FluentAssertions;
using NSubstitute;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Tasks;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tests.Tasks;

public class CreateTaskHandlerTests
{
    private readonly ITaskRepository _taskRepository = Substitute.For<ITaskRepository>();
    private readonly CreateTaskHandler _sut;

    public CreateTaskHandlerTests()
    {
        _sut = new CreateTaskHandler(_taskRepository);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task HandleAsync_throws_when_title_is_missing(string title)
    {
        var command = new CreateTaskCommand(
            Guid.NewGuid(),
            title,
            null,
            TaskStatus.Pending,
            DateOnly.FromDateTime(DateTime.UtcNow));

        var act = () => _sut.HandleAsync(command);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Title is required*");

        await _taskRepository.DidNotReceive().AddAsync(Arg.Any<TaskItem>());
    }

    [Fact]
    public async Task HandleAsync_throws_when_due_date_is_in_the_past()
    {
        var command = new CreateTaskCommand(
            Guid.NewGuid(),
            "Valid title",
            null,
            TaskStatus.Pending,
            DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1));

        var act = () => _sut.HandleAsync(command);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Due date cannot be in the past*");

        await _taskRepository.DidNotReceive().AddAsync(Arg.Any<TaskItem>());
    }

    [Fact]
    public async Task HandleAsync_creates_task_when_valid()
    {
        var userId = Guid.NewGuid();
        var dueDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(7);
        var command = new CreateTaskCommand(
            userId,
            "My task",
            "Description",
            TaskStatus.Pending,
            dueDate);

        var result = await _sut.HandleAsync(command);

        result.Title.Should().Be("My task");
        result.Description.Should().Be("Description");
        result.Status.Should().Be(TaskStatus.Pending);
        result.DueDate.Should().Be(dueDate);

        await _taskRepository.Received(1).AddAsync(
            Arg.Is<TaskItem>(t => t.UserId == userId && t.Title == "My task"),
            Arg.Any<CancellationToken>());
    }
}
