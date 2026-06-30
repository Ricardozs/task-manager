using FluentValidation;
using TaskManager.Application.Auth.Commands;

namespace TaskManager.Application.Auth.Validators;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(command => command)
            .Must(command =>
                !string.IsNullOrWhiteSpace(command.Email) &&
                !string.IsNullOrWhiteSpace(command.Password))
            .WithMessage("Email and password are required.");
    }
}
