using BackendService.Data.Domain;
using BackendService.Dtos;
using CustomLibrary.Helper;

namespace BackendService.Application.Core.IRepositories
{
    public interface IProductVariantRepository
    {
        ValueTask<ResponseBaseViewModel> SubmitProductVariant(ProductVariantDto productVariantDto, string? fileName);
        ValueTask<IList<MsProductVariant>> GetMsProductVariants();
        ValueTask<ResponseBaseViewModel> UpdateProductVariant(string id, ProductVariantDto productVariantDto, string? fileName = null);
    }
}
