using BackendService.Application.Core.IRepositories;
using BackendService.Dtos;
using CustomLibrary.Helper;
using CustomLibrary.Settings;
using ImageMagick;
using Microsoft.Extensions.Options;

namespace BackendService.Application.Core.Repositories
{
    public class UploadRepository : IUploadRepository
    {
        private readonly MediaSetting _mediaSetting;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadRepository(IOptions<MediaSetting> mediaSetting
            , IWebHostEnvironment webHostEnvironment)
        {
            _mediaSetting = mediaSetting.Value.FilePath is not null ? mediaSetting.Value : throw new ArgumentNullException(nameof(mediaSetting.Value.FilePath)); ;
            _webHostEnvironment = webHostEnvironment;
        }
        public async ValueTask<FileUploadResponse> UploadSingleFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var filename = Path.GetRandomFileName().Replace(".", string.Empty);
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, _mediaSetting.FilePath, DateTime.Now.Year.ToString(), extension.Substring(1));

            try
            {
                var result = await StoreFileOnDisk(file, path, filename, extension);
                return result;
            }
            catch (MagickException ex)
            {
                System.IO.File.Delete(path);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<FileUploadResponse> StoreFileOnDisk(IFormFile file, string path, string filename, string extension)
        {
            Directory.CreateDirectory(path);
            path = Path.Combine(path, filename + extension);

            using (var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            var result = new FileUploadResponse()
            {
                Filename = path.ConvertToUri(_mediaSetting.FilePath)
            };

            if (_mediaSetting.ThumbnailForExtensions is not null && _mediaSetting.ThumbnailForExtensions.Contains(extension))
            {
                var thumbPath = path.Insert(path.IndexOf(extension), "_thumb");
                CreateThumb(file, thumbPath);
                result.Thumbnail = thumbPath.ConvertToUri(_mediaSetting.FilePath);
            }
            return result;
        }

        private void CreateThumb(IFormFile file, string thumbPath)
        {
            using (var thumb = new MagickImage(file.OpenReadStream()))
            {
                thumb.Thumbnail(new MagickGeometry(100, 100));
                thumb.WriteAsync(thumbPath);
            }
        }
    }

    public static class UploadExtensions
    {
        public static string? ConvertToUri(this string? path, string separator)
        {
            return "media" + path?.Substring(path.IndexOf(separator) + separator.Length).Replace("\\", "/");
        }
    }
}
