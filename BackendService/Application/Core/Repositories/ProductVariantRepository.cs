using BackendService.Application.Core.IRepositories;
using BackendService.Data;
using BackendService.Data.Domain;
using BackendService.Dtos;
using CustomLibrary.Helper;
using CustomLibrary.Services;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Application.Core.Repositories
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ProductVariantRepository(ApplicationDbContext context
            ,IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async ValueTask<IList<MsProductVariant>> GetMsProductVariants()
        {
            var result = await _context.MsProductVariants.Where(x => x.IsActive == true)
                .ToListAsync();

            return result;
        }

        public async ValueTask<ResponseBaseViewModel> SubmitProductVariant(ProductVariantDto productVariantDto, string? fileName)
        {
            var response = new ResponseBaseViewModel();
            var generateCode = GenerateCodeNumber(productVariantDto.MsProductId);
            await using var transaction = _context.Database.BeginTransaction();
            try
            {
                var productVariant = new MsProductVariant()
                {
                    Name = productVariantDto.Name,
                    MsProductId = productVariantDto.MsProductId,
                    Code = generateCode,
                    Qty = productVariantDto.Qty,
                    Price = productVariantDto.Price,
                    ImageLocation = string.IsNullOrEmpty(fileName)?string.Empty : fileName,
                    CreatedDate = DateTime.UtcNow,
                    CreatedUser = _identityService.GetUserId(),
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUser = _identityService.GetUserId(),
                    IsActive = true,
                    IsDelete = false,
                };

                await _context.MsProductVariants.AddAsync(productVariant);
                transaction.Commit();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                response.IsError = true;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        private string GenerateCodeNumber(Guid? msProductId)
        {
            var findFormatByProductId = _context.MsProducts.FirstOrDefault(x => x.Id == msProductId);
            var totalCountByProductId = _context.MsProducts.Where(x => x.Id == msProductId).Count();
            var continuousNumber = (totalCountByProductId + 1).ToString("D3");

            var newFormat = findFormatByProductId?.Plu + continuousNumber;
            
            return newFormat;
        }

        public async ValueTask<ResponseBaseViewModel> UpdateProductVariant(string id, ProductVariantDto productVariantDto, string? fileName = null)
        {
            var response = new ResponseBaseViewModel();
            await using var transaction = _context.Database.BeginTransaction();
            try
            {
                var existing = await _context.MsProductVariants.FirstOrDefaultAsync(x => x.Id.ToString() == id);

                existing.Name = productVariantDto.Name;
                existing.Qty = productVariantDto.Qty;
                existing.Price = productVariantDto.Price;
                existing.ImageLocation = string.IsNullOrEmpty(fileName) ? string.Empty : fileName;
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
