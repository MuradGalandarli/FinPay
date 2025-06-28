using System.Diagnostics;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var request = context.Request;
        var userIp = context.Connection.RemoteIpAddress?.ToString();
        var user = context.User?.Identity?.Name ?? "Anonymous";

        _logger.LogInformation("Gələn sorğu: {method} {path} — User: {user}, IP: {ip}",
            request.Method, request.Path, user, userIp);

        await _next(context);

        stopwatch.Stop();

        _logger.LogInformation("Cavab: {statusCode} — {elapsed} ms",
            context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
    }
}
