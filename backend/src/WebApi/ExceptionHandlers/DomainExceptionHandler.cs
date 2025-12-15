using Application.Exceptions;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.ExceptionHandlers;

public sealed class DomainExceptionHandler
{
    public static bool CanHandle(Exception ex)
        => ex is DomainException;

    public static async Task HandleAsync(
        HttpContext context,
        DomainException exception,
        CancellationToken ct)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "Business Rule Violation",
            Detail = exception.Message
        };

        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        await context.Response.WriteAsJsonAsync(problemDetails, ct);
    }
}
