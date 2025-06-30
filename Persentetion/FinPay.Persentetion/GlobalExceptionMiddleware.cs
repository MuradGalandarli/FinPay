
    using System.Net;
    using System.Text.Json;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using FluentValidation; // əgər FluentValidation istifadə edirsənsə

    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Növbəti middleware və ya controller-a keç
            }
            catch (FinPay.Application.Exceptions.ValidationException ex)
            {
                _logger.LogWarning("Validation error: {@Errors}", ex.Errors);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var response = new
                {
                    errors = ex.Errors
                };



                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    error = "Internal Server Error",
                    detail = ex.Message // isteğe bağlı, prod-da gizlədə bilərsən
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }

