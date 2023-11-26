using BackendService.Data.Domain;
using BackendService.Dtos;
using CustomLibrary.Helper;

namespace BackendService.Application.Core.IRepositories
{
    public interface IProductRepository
    {
        ValueTask<ResponseBaseViewModel> SubmitProduct(ProductDto productDto);
        ValueTask<IList<MsProduct>> GetMsProducts();
        ValueTask<ResponseBaseViewModel> UpdateProduct(string id, ProductDto productDto);
    }
}
