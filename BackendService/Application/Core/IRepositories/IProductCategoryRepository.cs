using BackendService.Data.Domain;
using BackendService.Dtos;
using CustomLibrary.Helper;

namespace BackendService.Application.Core.IRepositories
{
    public interface IProductCategoryRepository
    {
        ValueTask<ResponseBaseViewModel> SubmitProductCategory(ProductCategoryDto productCategoryDto);
        ValueTask<IList<MsProductCategory>> GetProductCategories();
        ValueTask<ResponseBaseViewModel> UpdateProductCategory(string id, ProductCategoryDto productCategoryDto);
    }
}
