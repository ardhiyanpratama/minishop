using CustomLibrary.Adapter;
using CustomLibrary.Exceptions;
using CustomLibrary.Helper;
using CustomLibrary.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomLibrary.Middlewares
{
    public class MediaValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerAdapter<MediaValidationMiddleware> _loggerAdapter;
        private readonly MediaSetting _mediaSetting;

        public MediaValidationMiddleware(RequestDelegate next, IOptions<MediaSetting> mediaSetting, ILogger<MediaValidationMiddleware> logger)
        {
            _next = next;
            _loggerAdapter = new LoggerAdapter<MediaValidationMiddleware>(logger);
            _mediaSetting = mediaSetting.Value.FilePath is not null ? mediaSetting.Value : throw new ArgumentNullException(nameof(mediaSetting));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.ContentType is not null && httpContext.Request.ContentType.Contains("multipart/form-data"))
            {
                ValidateOptions(httpContext.Request.Form.Files);
            }

            await _next(httpContext);
        }

        private void ValidateOptions(IFormFileCollection fileCollection)
        {
            try
            {
                var totalSize = fileCollection.Sum(f => f.Length);
                if (totalSize > _mediaSetting.MultipleUpload.SizeLimit)
                {
                    throw new FileSizeOverLimitException();
                }

                foreach (var file in fileCollection)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!_mediaSetting.PermittedExtensions.Contains(extension.ToLower()))
                    {
                        throw new FileExtensionsProhibitedException();
                    }

                    if (!FileSignature.IsSigned(file.OpenReadStream(), extension, out var headerBytesAsString))
                    {
                        throw new FileSignatureErrorException("File Rusak / Tidak Dikenali");
                    }

                    if (file.Length <= 0)
                    {
                        throw new FileUploadException(ResponseMessageExtensions.File.SizeNotValid);
                    }

                    if (_mediaSetting.FileSizeLimits.ContainsKey(extension.ToLower()) && _mediaSetting.FileSizeLimits.TryGetValue(extension.ToLower(), out int sizeLimit)
                        && sizeLimit < file.Length)
                    {
                        throw new FileSizeOverLimitException();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                _loggerAdapter.LogWarning("ArgumentExceptionError: {0}", ex.Message);
                throw;
            }
            catch (OverflowException ex)
            {
                _loggerAdapter.LogWarning("OverflowExceptionError: {0}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _loggerAdapter.LogWarning("{0} Error {1}", ex.GetType(), ex.Message);
                throw;
            }
        }
    }

    public static class MediaValidationMiddlewareExtensions
    {
        public static void UseMediaValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<MediaValidationMiddleware>();
        }
    }
}
