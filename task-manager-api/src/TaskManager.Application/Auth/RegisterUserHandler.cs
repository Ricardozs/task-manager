using FluentValidation;
using TaskManager.Application.Auth.Commands;
using TaskManager.Application.Auth.Dtos;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Common.Validation;
using TaskManager.Domain.Entities;
namespace TaskManager.Application.Auth;

public class RegisterUserHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IValidator<RegisterUserCommand> validator)
{
    public async Task<AuthResponse> HandleAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        await validator.ThrowIfInvalidAsync(command, cancellationToken);

        var normalizedEmail = command.Email.Trim().ToLowerInvariant();

        if (await userRepository.EmailExistsAsync(normalizedEmail, cancellationToken))
            throw new DuplicateEmailException(normalizedEmail);

        var passwordHash = passwordHasher.Hash(command.Password);
        var user = User.Create(normalizedEmail, passwordHash, DateTime.UtcNow);

        await userRepository.AddAsync(user, cancellationToken);

        var token = jwtTokenGenerator.GenerateToken(user.Id, user.Email);
        return new AuthResponse(token, user.Email);
    }
}
