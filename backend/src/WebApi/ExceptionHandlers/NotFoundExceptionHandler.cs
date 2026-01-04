using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.ExceptionHandlers;

public sealed class NotFoundExceptionHandler
{
    public static bool CanHandle(Exception ex)
        => ex is EntityNotFoundException;

    public static async Task HandleAsync(
        HttpContext context,
        EntityNotFoundException exception,
        CancellationToken ct)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Resource Not Found",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5"
        };

        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsJsonAsync(problemDetails, ct);
    }
}
