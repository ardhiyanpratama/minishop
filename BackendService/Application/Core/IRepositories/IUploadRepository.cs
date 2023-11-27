using BackendService.Dtos;

namespace BackendService.Application.Core.IRepositories
{
    public interface IUploadRepository
    {
        ValueTask<FileUploadResponse> UploadSingleFile(IFormFile file);
    }
}
