using FluentValidation;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Validators;

public class UpdateTaskValidationContextValidator : AbstractValidator<UpdateTaskValidationContext>
{
    public UpdateTaskValidationContextValidator()
    {
        RuleFor(context => context.Command.Title)
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .WithMessage("Title is required.");

        RuleFor(context => context.Command.Status)
            .Must((context, status) =>
                context.ExistingTask.Status != TaskStatus.Completed || status == TaskStatus.Completed)
            .WithMessage("Completed tasks cannot revert to another status.");
    }
}
