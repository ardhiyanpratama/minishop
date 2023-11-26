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
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IProductRepository _productRepository;

        public ProductController(ApplicationDbContext context
            , IIdentityService identityService
            , IProductRepository productRepository)
        {
            _context = context;
            _identityService = identityService;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedItemsViewModel<MsProduct>>> GetAll(
           [FromQuery] PaginationFilter filter,
           [FromQuery] string? name,
           [FromQuery] string? plu,
           [FromQuery] bool? activeOnly,
           CancellationToken cancellationToken)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var listQuery = _context.MsProducts
                .Where(x => x.IsDelete == false);

            if (activeOnly is not null && activeOnly == true)
            {
                listQuery = listQuery.Where(e => e.IsActive == true);
            }
            if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(plu))
            {
                listQuery = listQuery.Where(e => e.Name.ToLower().Contains(name.ToLower()) || e.Plu.ToLower().Contains(plu.ToLower()));
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
                return Ok(new PaginatedItemsViewModel<MsProduct>(validFilter.PageNumber, validFilter.PageSize, totalItems, new List<MsProduct>()));
            }

            var result = listing.Adapt<List<MsProduct>>();

            var viewModel = new PaginatedItemsViewModel<MsProduct>(validFilter.PageNumber, validFilter.PageSize, totalItems, result);

            return Ok(viewModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MsProduct>> GetSingle([FromRoute] string id, CancellationToken cancellationToken)
        {
            var existing = await _context.MsProducts
                .FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);

            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Product.ProductNotFound);
            }

            return Ok(existing);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDto input, CancellationToken cancellationToken)
        {
            if (await _context.MsProducts.AnyAsync(o => o.Name.ToLower() == input.Name.ToLower() && o.IsDelete == false, cancellationToken))
            {
                throw new AppException(ResponseMessageExtensions.Product.ProductAlreadyExist);
            }

            var result = await _productRepository.SubmitProduct(input);

            if (result.IsError)
            {
                throw new AppException(ResponseMessageExtensions.Database.WriteFailed);
            }

            return this.OkResponse(ResponseMessageExtensions.Database.WriteSuccess);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] ProductDto input, CancellationToken cancellationToken)
        {
            var existing = await _context.MsProducts.FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);
            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Product.ProductNotFound);
            }

            var updateResult = await _productRepository.UpdateProduct(id, input);

            if (updateResult.IsError)
            {
                throw new AppException(ResponseMessageExtensions.Database.UpdateFailed);
            }

            return this.OkResponse(ResponseMessageExtensions.Database.UpdateSuccess);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> IsActived([FromRoute] string id, [FromBody] ComponentBase componentBase, CancellationToken cancellationToken)
        {

            var existing = await _context.MsProducts.FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);
            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Product.ProductNotFound);
            }

            //check if used in product variant
            var checkIfUsed = await _context.MsProductVariants
                .FirstOrDefaultAsync(e => e.MsProductId == existing.Id && e.IsDelete == false && e.IsActive == true, cancellationToken);

            if (checkIfUsed is not null)
            {
                throw new AppException(ResponseMessageExtensions.Product.ProductUsedInVariant);
            }

            existing.UpdatedDate = DateTimeOffset.Now;
            existing.UpdatedUser = _identityService.GetUserId();
            existing.IsActive = componentBase.IsActive;

            _context.MsProducts.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return this.OkResponse(ResponseMessageExtensions.Database.UpdateSuccess);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
        {
            var existing = await _context.MsProducts.FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);
            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Product.ProductNotFound);
            }

            //check if used in product variant
            var checkIfUsed = await _context.MsProductVariants
                .FirstOrDefaultAsync(e => e.MsProductId == existing.Id && e.IsDelete == false && e.IsActive == true, cancellationToken);

            if (checkIfUsed is not null)
            {
                throw new AppException(ResponseMessageExtensions.Product.ProductUsedInVariant);
            }

            existing.UpdatedDate = DateTimeOffset.Now;
            existing.UpdatedUser = _identityService.GetUsername();
            existing.IsDelete = true;

            _context.MsProducts.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return this.OkResponse(ResponseMessageExtensions.Database.DeleteSuccess);
        }
    }
}
