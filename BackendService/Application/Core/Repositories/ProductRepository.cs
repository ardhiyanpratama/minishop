using BackendService.Application.Core.IRepositories;
using BackendService.Data;
using BackendService.Data.Domain;
using BackendService.Dtos;
using CustomLibrary.Helper;
using CustomLibrary.Services;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Application.Core.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IIdentityService _identityService;

        public ProductRepository(ApplicationDbContext applicationDbContext
            , IIdentityService identityService)
        {
            _applicationDbContext = applicationDbContext;
            _identityService = identityService;
        }
        public async ValueTask<IList<MsProduct>> GetMsProducts()
        {
            var result = await _applicationDbContext.MsProducts.Where(x => x.IsActive == true)
                .ToListAsync();
            
            return result;
        }

        public async ValueTask<ResponseBaseViewModel> SubmitProduct(ProductDto productDto)
        {
            var response = new ResponseBaseViewModel();
            await using var transaction = _applicationDbContext.Database.BeginTransaction();
            try
            {
                var product = new MsProduct()
                {
                    Name = productDto.Name,
                    Plu = GeneratePluNumber(),
                    CreatedDate = DateTime.UtcNow,
                    CreatedUser = _identityService.GetUserId(),
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUser = _identityService.GetUserId(),
                };

                await _applicationDbContext.MsProducts.AddAsync(product);
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

        private string GeneratePluNumber()
        {
            var findCountAllData = _applicationDbContext.MsProducts.Count();
            var continuousNumber = (findCountAllData + 1).ToString("D5");

            var newFormat = "PDCT" + continuousNumber;
            return newFormat;
        }

        public async ValueTask<ResponseBaseViewModel> UpdateProduct(string id, ProductDto productDto)
        {
            var response = new ResponseBaseViewModel();
            await using var transaction = _applicationDbContext.Database.BeginTransaction();
            try
            {
                var existing = await _applicationDbContext.MsProducts.FirstOrDefaultAsync(x => x.Id.ToString() == id);

                existing.Name = productDto.Name;
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
