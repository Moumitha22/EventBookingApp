using System.Text.Json;
using EventBookingApi.Exceptions;
using EventBookingApi.Interfaces;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IApiResponseMapper _responseMapper;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IApiResponseMapper responseMapper)
        {
            _next = next;
            _logger = logger;
            _responseMapper = responseMapper;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                BadRequestException => (StatusCodes.Status400BadRequest, $"Validation failed: {exception.Message}"),
                UnauthorizedException => (StatusCodes.Status401Unauthorized, $"Unauthorized: {exception.Message}"),
                NotFoundException => (StatusCodes.Status404NotFound, $"Not found: {exception.Message}"),
                _ => (StatusCodes.Status500InternalServerError, $"Server error: {exception.Message}")
            };

            var errors = new Dictionary<string, string[]>
            {
                {
                    "general", new[]
                    {
                        exception is BadRequestException or UnauthorizedException or NotFoundException 
                            ? exception.Message
                            : "An unexpected error occurred."
                    }
                }
            };

            var errorResponse = _responseMapper.MapToErrorResponse<object>(statusCode, message, errors);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }
    }
}
