using TaskManager.Application.Auth.Commands;
using TaskManager.Application.Auth.Dtos;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Interfaces;

namespace TaskManager.Application.Auth;

public class LoginUserHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator)
{
    public async Task<AuthResponse> HandleAsync(
        LoginUserCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Password))
            throw new ValidationException("Email and password are required.");

        var normalizedEmail = command.Email.Trim().ToLowerInvariant();
        var user = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null || !passwordHasher.Verify(command.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

        var token = jwtTokenGenerator.GenerateToken(user.Id, user.Email);
        return new AuthResponse(token, user.Email);
    }
}
