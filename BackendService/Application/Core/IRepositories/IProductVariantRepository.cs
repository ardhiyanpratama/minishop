using BackendService.Data.Domain;
using BackendService.Dtos;
using CustomLibrary.Helper;

namespace BackendService.Application.Core.IRepositories
{
    public interface IProductVariantRepository
    {
        ValueTask<ResponseBaseViewModel> SubmitProductVariant(ProductVariantDto productVariantDto);
        ValueTask<IList<MsProductVariant>> GetMsProductVariants();
        ValueTask<ResponseBaseViewModel> UpdateProductVariant(string id, ProductVariantDto productVariantDto);
    }
}
