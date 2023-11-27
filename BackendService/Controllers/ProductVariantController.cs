using BackendService.Application.Core.IRepositories;
using BackendService.Application.Core.Repositories;
using BackendService.Data;
using BackendService.Data.Domain;
using BackendService.Dtos;
using CustomLibrary.Exceptions;
using CustomLibrary.Helper.Api;
using CustomLibrary.Helper;
using CustomLibrary.Services;
using CustomLibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace BackendService.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductVariantController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IProductVariantRepository _productVariantRepository;

        public ProductVariantController(ApplicationDbContext context
            ,IIdentityService identityService
            ,IProductVariantRepository productVariantRepository)
        {
            _context = context;
            _identityService = identityService;
            _productVariantRepository = productVariantRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedItemsViewModel<MsProductVariant>>> GetAll(
           [FromQuery] PaginationFilter filter,
           [FromQuery] string? name,
           [FromQuery] string? code,
           [FromQuery] bool? activeOnly,
           CancellationToken cancellationToken)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var listQuery = _context.MsProductVariants
                .Where(x => x.IsDelete == false);

            if (activeOnly is not null && activeOnly == true)
            {
                listQuery = listQuery.Where(e => e.IsActive == true);
            }
            if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(code))
            {
                listQuery = listQuery.Where(e => e.Name.ToLower().Contains(name.ToLower()) || e.Code.ToLower().Contains(code.ToLower()));
            }

            var totalItems = listQuery.Count();
            var listing = await listQuery
                .OrderByDescending(e => e.CreatedDate)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (listing.Count == 0)
            {
                return Ok(new PaginatedItemsViewModel<MsProductVariant>(validFilter.PageNumber, validFilter.PageSize, totalItems, new List<MsProductVariant>()));
            }

            var result = listing.Adapt<List<MsProductVariant>>();

            var viewModel = new PaginatedItemsViewModel<MsProductVariant>(validFilter.PageNumber, validFilter.PageSize, totalItems, result);

            return Ok(viewModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MsProductVariant>> GetSingle([FromRoute] string id, CancellationToken cancellationToken)
        {
            var existing = await _context.MsProductVariants
                .FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);

            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Variant.ProductVariantNotFound);
            }

            return Ok(existing);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductVariantDto input, CancellationToken cancellationToken)
        {
            if (await _context.MsProductVariants.AnyAsync(o => o.Name.ToLower() == input.Name.ToLower() && o.IsDelete == false, cancellationToken))
            {
                throw new AppException(ResponseMessageExtensions.Variant.ProductVariantAlreadyExist);
            }

            var result = await _productVariantRepository.SubmitProductVariant(input);

            if (result.IsError)
            {
                throw new AppException(ResponseMessageExtensions.Database.WriteFailed);
            }

            return this.OkResponse(ResponseMessageExtensions.Database.WriteSuccess);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] ProductVariantDto input, CancellationToken cancellationToken)
        {
            var existing = await _context.MsProductVariants.FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);
            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Variant.ProductVariantNotFound);
            }

            var updateResult = await _productVariantRepository.UpdateProductVariant(id, input);

            if (updateResult.IsError)
            {
                throw new AppException(ResponseMessageExtensions.Database.UpdateFailed);
            }

            return this.OkResponse(ResponseMessageExtensions.Database.UpdateSuccess);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> IsActived([FromRoute] string id, [FromBody] ComponentBase componentBase, CancellationToken cancellationToken)
        {

            var existing = await _context.MsProductVariants.FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);
            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Variant.ProductVariantNotFound);
            }

            //check if used in product variant
            var checkIfUsed = await _context.TransactionDetails
                .FirstOrDefaultAsync(e => e.MsProductVariantId == existing.Id && e.IsDelete == false && e.IsActive == true, cancellationToken);

            if (checkIfUsed is not null)
            {
                throw new AppException(ResponseMessageExtensions.Variant.ProductVariantUsedInTransactionDetail);
            }

            existing.UpdatedDate = DateTimeOffset.Now;
            existing.UpdatedUser = _identityService.GetUserId();
            existing.IsActive = componentBase.IsActive;

            _context.MsProductVariants.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return this.OkResponse(ResponseMessageExtensions.Database.UpdateSuccess);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
        {
            var existing = await _context.MsProductVariants.FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);
            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Variant.ProductVariantNotFound);
            }

            //check if used in product variant
            var checkIfUsed = await _context.TransactionDetails
                .FirstOrDefaultAsync(e => e.MsProductVariantId == existing.Id && e.IsDelete == false && e.IsActive == true, cancellationToken);

            if (checkIfUsed is not null)
            {
                throw new AppException(ResponseMessageExtensions.Variant.ProductVariantUsedInTransactionDetail);
            }

            existing.UpdatedDate = DateTimeOffset.Now;
            existing.UpdatedUser = _identityService.GetUsername();
            existing.IsDelete = true;

            _context.MsProductVariants.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return this.OkResponse(ResponseMessageExtensions.Database.DeleteSuccess);
        }
    }
}
