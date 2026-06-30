using FluentValidation;
using TaskManager.Application.Auth.Commands;
using TaskManager.Application.Auth.Dtos;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Common.Validation;

namespace TaskManager.Application.Auth;

public class LoginUserHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IValidator<LoginUserCommand> validator)
{
    public async Task<AuthResponse> HandleAsync(
        LoginUserCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ThrowIfInvalidAsync(command, cancellationToken);

        var normalizedEmail = command.Email.Trim().ToLowerInvariant();
        var user = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null || !passwordHasher.Verify(command.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

        var token = jwtTokenGenerator.GenerateToken(user.Id, user.Email);
        return new AuthResponse(token, user.Email);
    }
}
