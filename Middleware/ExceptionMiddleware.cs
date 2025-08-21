using Stickers.Models.Exceptions;
using System.Text.Json;

namespace Stickers.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error in the application");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            IdentityRegistrationException => new { message = exception.Message, statusCode = 400 },
            UsernameAlreadyExistsException => new { message = "Username already exists", statusCode = 400 },
            UnauthorizedException => new { message = "User not authenticated", statusCode = 401 },
            ForbiddenException => new { message = "User not authorized", statusCode = 403 },
            ConflictException => new { message = exception.Message, statusCode = 409 },
            NotFoundException => new { message = exception.Message, statusCode = 404 },
            _ => new { message = "Internal server error", statusCode = 500 }
        };

        context.Response.StatusCode = response.statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
