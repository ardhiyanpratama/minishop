using BackendService.Application.Core.IRepositories;
using BackendService.Data;
using BackendService.Data.Domain;
using BackendService.Dtos;
using CustomLibrary.Exceptions;
using CustomLibrary.Helper;
using CustomLibrary.Helper.Api;
using CustomLibrary.Services;
using CustomLibrary.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductCategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryController(ApplicationDbContext context
            ,IIdentityService identityService
            ,IProductCategoryRepository productCategoryRepository)
        {
            _context = context;
            _identityService = identityService;
            _productCategoryRepository = productCategoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedItemsViewModel<MsProductCategory>>> GetAll(
           [FromQuery] PaginationFilter filter,
           [FromQuery] string? name,
           [FromQuery] bool? activeOnly,
           CancellationToken cancellationToken)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var listQuery = _context.MsProductCategories
                .Where(x => x.IsDelete == false);

            if (activeOnly is not null && activeOnly == true)
            {
                listQuery = listQuery.Where(e => e.IsActive == true);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                listQuery = listQuery.Where(e => e.Name.ToLower().Contains(name.ToLower()));
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
                return Ok(new PaginatedItemsViewModel<MsProductCategory>(validFilter.PageNumber, validFilter.PageSize, totalItems, new List<MsProductCategory>()));
            }

            var result = listing.Adapt<List<MsProductCategory>>();

            var viewModel = new PaginatedItemsViewModel<MsProductCategory>(validFilter.PageNumber, validFilter.PageSize, totalItems, result);

            return Ok(viewModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MsProductCategory>> GetSingle([FromRoute] string id, CancellationToken cancellationToken)
        {
            var existing = await _context.MsProductCategories
                .FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);

            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Category.CategoryNotFound);
            }

            return Ok(existing);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductCategoryDto input, CancellationToken cancellationToken)
        {
            if (await _context.MsProductCategories.AnyAsync(o => o.Name!.ToLower() == input.Name.ToLower() && o.IsDelete == false, cancellationToken))
            {
                throw new AppException(ResponseMessageExtensions.Category.CategoryAlreadyExist);
            }

            var result = await _productCategoryRepository.SubmitProductCategory(input);

            if (result.IsError)
            {
                throw new AppException(ResponseMessageExtensions.Database.WriteFailed);
            }

            return this.OkResponse(ResponseMessageExtensions.Database.WriteSuccess);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] ProductCategoryDto input, CancellationToken cancellationToken)
        {
            var existing = await _context.MsProductCategories.FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);
            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Category.CategoryNotFound);
            }

            var updateResult = await _productCategoryRepository.UpdateProductCategory(id, input);

            if (updateResult.IsError) {
                throw new AppException(ResponseMessageExtensions.Database.UpdateFailed);
            }

            return this.OkResponse(ResponseMessageExtensions.Database.UpdateSuccess);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> IsActived([FromRoute] string id, [FromBody] ComponentBase componentBase, CancellationToken cancellationToken)
        {

            var existing = await _context.MsProductCategories.FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);
            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Category.CategoryNotFound);
            }

            //check if used in product
            var checkIfUsed = await _context.MsProducts
                .FirstOrDefaultAsync(e => e.MsProductCategoryId == existing.Id && e.IsDelete == false && e.IsActive == true, cancellationToken);

            if (checkIfUsed is not null)
            {
                throw new AppException(ResponseMessageExtensions.Category.CategoryUsedInProduct);
            }

            existing.UpdatedDate = DateTimeOffset.Now;
            existing.UpdatedUser = _identityService.GetUserId();
            existing.IsActive = componentBase.IsActive;

            _context.MsProductCategories.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return this.OkResponse(ResponseMessageExtensions.Database.UpdateSuccess);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
        {
            var existing = await _context.MsProductCategories.FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);
            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Category.CategoryNotFound);
            }

            //check if used in product
            var checkIfUsed = await _context.MsProducts
                .FirstOrDefaultAsync(e => e.MsProductCategoryId == existing.Id && e.IsDelete == false && e.IsActive == true, cancellationToken);

            if (checkIfUsed is not null)
            {
                throw new AppException(ResponseMessageExtensions.Category.CategoryUsedInProduct);
            }

            existing.UpdatedDate = DateTimeOffset.Now;
            existing.UpdatedUser = _identityService.GetUsername();
            existing.IsDelete = true;

            _context.MsProductCategories.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return this.OkResponse(ResponseMessageExtensions.Database.DeleteSuccess);
        }
    }
}
