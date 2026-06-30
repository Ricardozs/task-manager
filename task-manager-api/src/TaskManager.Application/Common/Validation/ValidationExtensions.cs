using FluentValidation;
using TaskManager.Application.Common.Exceptions;

namespace TaskManager.Application.Common.Validation;

public static class ValidationExtensions
{
    public static async Task ThrowIfInvalidAsync<T>(
        this IValidator<T> validator,
        T instance,
        CancellationToken cancellationToken = default)
    {
        var result = await validator.ValidateAsync(instance, cancellationToken);
        if (!result.IsValid)
        {
            var message = string.Join(' ', result.Errors.Select(error => error.ErrorMessage));
            throw new Exceptions.ValidationException(message);
        }
    }
}
