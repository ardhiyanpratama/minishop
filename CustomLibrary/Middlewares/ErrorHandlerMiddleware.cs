using CustomLibrary.Adapter;
using CustomLibrary.Exceptions;
using CustomLibrary.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomLibrary.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerAdapter<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = new LoggerAdapter<ErrorHandlerMiddleware>(logger);
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (AggregateException ex)
            {
                await ProcessExceptionMessage(ex.InnerException, httpContext);
            }
            catch (Exception ex)
            {
                await ProcessExceptionMessage(ex, httpContext);
            }
        }

        private async Task ProcessExceptionMessage(Exception ex, HttpContext httpContext)
        {
            switch (ex)
            {
                case AppException appException:
                    _logger.LogInformation(ex.Message);
                    await httpContext.Response.BadRequestResponse(ex.Message, appException.ErrorNote, appException.ErrorData);
                    break;
                case ExternalServiceException externalServiceException:
                    _logger.LogInformation("HttpStatusCode: {0}, Message: {1}", externalServiceException.StatusCode, externalServiceException.Message);
                    await httpContext.Response.ServiceUnavailableResponse($"Mohon Maaf Sedang Terjadi Kendala (Kode: {externalServiceException.StatusCode} {externalServiceException.ServiceName})");
                    break;
                case HttpRequestException httpRequestException:
                    _logger.LogInformation("HttpStatusCode: {0}, Message: {1}", httpRequestException.StatusCode, httpRequestException.Message);
                    await httpContext.Response.ServiceUnavailableResponse($"Mohon Maaf Sedang Terjadi Kendala (Kode: {httpRequestException.StatusCode})");
                    break;
                case ApplicationException:
                case ArgumentException:
                case BadHttpRequestException:
                case InvalidOperationException:
                    _logger.LogInformation("{0}: {1}", ex.GetType().Name, ex.Message);
                    await httpContext.Response.BadRequestResponse(ex.Message);
                    break;
                case FileSignatureErrorException:
                    await httpContext.Response.BadRequestResponse(ex.Message);
                    break;
                case UnauthorizedAccessException:
                    _logger.LogInformation(ex.Message);
                    await httpContext.Response.UnathourizedResponse(ex.Message);
                    break;
                case TimeoutException:
                    _logger.LogError(ex.Message);
                    await httpContext.Response.ServiceUnavailableResponse(ex.Message);
                    break;
                case KeyNotFoundException:
                    _logger.LogDebug(ex.Message);
                    await httpContext.Response.BadRequestResponse(string.IsNullOrWhiteSpace(ex.Message) ? ResponseMessageExtensions.Database.DataNotFound : ex.Message);
                    break;
                case MethodAccessException:
                    _logger.LogInformation(ex.Message);
                    await httpContext.Response.ForbiddedResponse();
                    break;
                default:
                    if (ex.InnerException is not null)
                    {
                        _logger.LogCritical(ex.InnerException.Message);
                        await httpContext.Response.InternalErrorResponse(ex.InnerException.Message);
                        break;
                    }
                    _logger.LogCritical(ex.Message);
                    await httpContext.Response.InternalErrorResponse(ex.Message);
                    break;
            }
        }
    }

    public static class ErrorHandlerMiddlewareExtensions
    {
        public static void UseErrorHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
