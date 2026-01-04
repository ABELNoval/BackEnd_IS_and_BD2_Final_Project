using Application.Exceptions;
using Domain.Exceptions;

namespace WebAPI.ExceptionHandlers;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        IWebHostEnvironment env,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _env = env;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            var ct = context.RequestAborted;

            if (ValidationExceptionHandler.CanHandle(ex))
            {
                await ValidationExceptionHandler.HandleAsync(
                    context, (ValidationException)ex, ct);
                return;
            }

            if (NotFoundExceptionHandler.CanHandle(ex))
            {
                await NotFoundExceptionHandler.HandleAsync(
                    context, (EntityNotFoundException)ex, ct);
                return;
            }

            if (DomainExceptionHandler.CanHandle(ex))
            {
                await DomainExceptionHandler.HandleAsync(
                    context, (DomainException)ex, ct);
                return;
            }

            await GlobalExceptionHandler.HandleAsync(
                context, ex, _env, ct);
        }
    }
}
