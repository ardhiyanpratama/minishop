using BackendService.Application.Core.IRepositories;
using BackendService.Data;
using BackendService.Data.Domain;
using BackendService.Dtos;
using CustomLibrary.Helper;
using CustomLibrary.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;
using static CustomLibrary.Helper.ResponseMessageExtensions;

namespace BackendService.Application.Core.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IIdentityService _identityService;

        public ProductCategoryRepository(ApplicationDbContext applicationDbContext
            ,IIdentityService identityService)
        {
            _applicationDbContext = applicationDbContext;
            _identityService = identityService;
        }
        public async ValueTask<IList<MsProductCategory>> GetProductCategories()
        {
            var result = await _applicationDbContext.MsProductCategories.Where(x => x.IsActive == true)
                .ToListAsync();
            
            return result;
        }

        public async ValueTask<ResponseBaseViewModel> SubmitProductCategory(ProductCategoryDto productCategoryDto)
        {
            var response = new ResponseBaseViewModel();
            await using var transaction = _applicationDbContext.Database.BeginTransaction();
            try
            {
                var productCategory = new MsProductCategory()
                {
                    Name = productCategoryDto.Name,
                    CreatedDate = DateTime.UtcNow,
                    CreatedUser = _identityService.GetUserId(),
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUser = _identityService.GetUserId(),
                };

                await _applicationDbContext.MsProductCategories.AddAsync(productCategory);
                await _applicationDbContext.SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public async ValueTask<ResponseBaseViewModel> UpdateProductCategory(string id, ProductCategoryDto productCategoryDto)
        {
            var response = new ResponseBaseViewModel();
            await using var transaction = _applicationDbContext.Database.BeginTransaction();
            try
            {
                var existing = await _applicationDbContext.MsProductCategories.FirstOrDefaultAsync(x => x.Id.ToString() == id);

                existing.Name = productCategoryDto.Name;
                existing.UpdatedDate = DateTime.UtcNow;
                existing.UpdatedUser = _identityService.GetUserId();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
