using Figurinhas.Models.Exceptions;
using System.Text.Json;

namespace Figurinhas.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
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
            _logger.LogError(ex, "Erro não tratado na aplicação");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            UnauthorizedException => new { message = "Usuário não autenticado", statusCode = 401 },
            ForbiddenException => new { message = "Usuário não autorizado", statusCode = 403 },
            ConflictException => new { message = exception.Message, statusCode = 409 },
            NotFoundException => new { message = exception.Message, statusCode = 404 },
            _ => new { message = "Erro interno do servidor", statusCode = 500 }
        };

        context.Response.StatusCode = response.statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
