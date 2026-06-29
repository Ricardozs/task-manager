using System.Text.RegularExpressions;
using TaskManager.Application.Auth.Commands;
using TaskManager.Application.Auth.Dtos;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Auth;

public partial class RegisterUserHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator)
{
    private const int MinPasswordLength = 8;

    public async Task<AuthResponse> HandleAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        Validate(command);

        var normalizedEmail = command.Email.Trim().ToLowerInvariant();

        if (await userRepository.EmailExistsAsync(normalizedEmail, cancellationToken))
            throw new DuplicateEmailException(normalizedEmail);

        var passwordHash = passwordHasher.Hash(command.Password);
        var user = User.Create(normalizedEmail, passwordHash, DateTime.UtcNow);

        await userRepository.AddAsync(user, cancellationToken);

        var token = jwtTokenGenerator.GenerateToken(user.Id, user.Email);
        return new AuthResponse(token, user.Email);
    }

    private static void Validate(RegisterUserCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Email))
            throw new ValidationException("Email is required.");

        if (!EmailRegex().IsMatch(command.Email))
            throw new ValidationException("Email format is invalid.");

        if (string.IsNullOrWhiteSpace(command.Password))
            throw new ValidationException("Password is required.");

        if (command.Password.Length < MinPasswordLength)
            throw new ValidationException($"Password must be at least {MinPasswordLength} characters.");
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
