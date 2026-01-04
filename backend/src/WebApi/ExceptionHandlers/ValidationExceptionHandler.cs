using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.ExceptionHandlers;

public sealed class ValidationExceptionHandler
{
    public static bool CanHandle(Exception ex)
        => ex is ValidationException;

    public static async Task HandleAsync(
        HttpContext context,
        ValidationException exception,
        CancellationToken ct)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Error",
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
        };

        problemDetails.Extensions["errors"] = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => char.ToLowerInvariant(g.Key[0]) + g.Key[1..],
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(problemDetails, ct);
    }
}
