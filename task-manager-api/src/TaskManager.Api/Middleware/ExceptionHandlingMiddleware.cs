using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Common.Exceptions;

namespace TaskManager.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, detail) = exception switch
        {
            ValidationException validation => (StatusCodes.Status400BadRequest, "Validation error", validation.Message),
            DuplicateEmailException duplicate => (StatusCodes.Status409Conflict, "Conflict", duplicate.Message),
            InvalidCredentialsException => (StatusCodes.Status401Unauthorized, "Unauthorized", exception.Message),
            NotFoundException => (StatusCodes.Status404NotFound, "Not found", exception.Message),
            ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden", exception.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized", exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Internal server error", "An unexpected error occurred.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unhandled exception");

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}
