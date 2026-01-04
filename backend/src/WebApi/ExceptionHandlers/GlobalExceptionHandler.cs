using Microsoft.AspNetCore.Mvc;

namespace WebAPI.ExceptionHandlers;

public sealed class GlobalExceptionHandler
{
    public static async Task HandleAsync(
        HttpContext context,
        Exception exception,
        IWebHostEnvironment env,
        CancellationToken ct)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            Detail = env.IsDevelopment()
                ? exception.ToString()
                : "An unexpected error occurred."
        };

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(problemDetails, ct);
    }
}
