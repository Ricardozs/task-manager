using FluentValidation;
using TaskManager.Application.Tasks.Commands;

namespace TaskManager.Application.Tasks.Validators;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(command => command.Title)
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .WithMessage("Title is required.");

        RuleFor(command => command.DueDate)
            .Must(dueDate => dueDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Due date cannot be in the past.");
    }
}
