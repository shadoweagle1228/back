using Microsoft.AspNetCore.Http;

namespace Infrastructure.Middleware;

public class ValidationRequestMiddleware : IMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationRequestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Method is "POST" or "PATCH" or "PUT" && !context.Request.Body.CanRead)
        {
            await context.Response.WriteAsync("Solicitud mal formada: el cuerpo de la solicitud no es legible.");
            throw new BadHttpRequestException("Solicitud mal formada: el cuerpo de la solicitud no es legible.");
        }
        await _next(context);
    }
}
