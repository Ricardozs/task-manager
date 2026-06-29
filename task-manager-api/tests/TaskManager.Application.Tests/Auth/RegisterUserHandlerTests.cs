using FluentAssertions;
using NSubstitute;
using TaskManager.Application.Auth;
using TaskManager.Application.Auth.Commands;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tests.Auth;

public class RegisterUserHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly IJwtTokenGenerator _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
    private readonly RegisterUserHandler _sut;

    public RegisterUserHandlerTests()
    {
        _passwordHasher.Hash(Arg.Any<string>()).Returns("hashed-password");
        _jwtTokenGenerator.GenerateToken(Arg.Any<Guid>(), Arg.Any<string>()).Returns("jwt-token");
        _sut = new RegisterUserHandler(_userRepository, _passwordHasher, _jwtTokenGenerator);
    }

    [Theory]
    [InlineData("short")]
    [InlineData("1234567")]
    public async Task HandleAsync_throws_when_password_is_too_short(string password)
    {
        var command = new RegisterUserCommand("user@example.com", password);

        var act = () => _sut.HandleAsync(command);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*at least 8 characters*");
    }

    [Fact]
    public async Task HandleAsync_throws_when_password_is_missing()
    {
        var command = new RegisterUserCommand("user@example.com", "");

        var act = () => _sut.HandleAsync(command);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Password is required*");
    }

    [Fact]
    public async Task HandleAsync_throws_duplicate_email_when_email_already_exists()
    {
        _userRepository.EmailExistsAsync("user@example.com").Returns(true);
        var command = new RegisterUserCommand("user@example.com", "validpassword");

        var act = () => _sut.HandleAsync(command);

        await act.Should().ThrowAsync<DuplicateEmailException>()
            .WithMessage("*user@example.com*");

        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task HandleAsync_registers_user_when_valid()
    {
        _userRepository.EmailExistsAsync("user@example.com").Returns(false);
        var command = new RegisterUserCommand("user@example.com", "validpassword");

        var result = await _sut.HandleAsync(command);

        result.Token.Should().Be("jwt-token");
        result.Email.Should().Be("user@example.com");
        await _userRepository.Received(1).AddAsync(
            Arg.Is<User>(u => u.Email == "user@example.com"),
            Arg.Any<CancellationToken>());
        _passwordHasher.Received(1).Hash("validpassword");
    }
}
