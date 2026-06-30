using System.Net;
using System.Text.Json;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Exceptions;
using WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.Kafka;
namespace WebApplicationMinimalAPINetCoreTreinamento.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            object errorResponse = new
            {
                Message = exception.Message,
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path,
                Method = context.Request.Method
            };

            switch (exception)
            {
                case EntityNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case EntityAlreadyExistsException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;

                case BusinessRuleException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case InvalidCepException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case FluentValidation.ValidationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = new
                    {
                        Message = "Validation failed",
                        Errors = ((FluentValidation.ValidationException)exception).Errors.Select(e => e.ErrorMessage),
                        Timestamp = DateTime.UtcNow,
                        Path = context.Request.Path,
                        Method = context.Request.Method
                    };
                    break;

                case KafkaProducerException:
                    response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    _logger.LogError(exception, "Kafka producer error");
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(exception, "Unhandled exception occurred");
                    errorResponse = new
                    {
                        Message = "An internal error occurred. Please try again later.",
                        Timestamp = DateTime.UtcNow,
                        Path = context.Request.Path,
                        Method = context.Request.Method,
                        // Only include details in development
                        Detail = exception.Message
                    };
                    break;
            }

            var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await response.WriteAsync(json);
        }
    }
}
