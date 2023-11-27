using BackendService.Application.Core.IRepositories;
using BackendService.Application.Core.Repositories;
using BackendService.Data;
using BackendService.Dtos;
using CustomLibrary.Exceptions;
using CustomLibrary.Helper;
using CustomLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ApplicationDbContext context
            ,IIdentityService identityService
            ,ITransactionRepository transactionRepository
            )
        {
            _context = context;
            _identityService = identityService;
            _transactionRepository = transactionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransactionDto input, CancellationToken cancellationToken)
        {
            var result = await _transactionRepository.SubmitTransaction(input);

            if (result.IsError)
            {
                throw new AppException(ResponseMessageExtensions.Database.WriteFailed);
            }

            return this.OkResponse(ResponseMessageExtensions.Database.WriteSuccess);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] TransactionDto input, CancellationToken cancellationToken)
        {
            var existing = await _context.Transactions
                .Include(x => x.TransactionDetails)
                .FirstOrDefaultAsync(e => e.Id.ToString() == id, cancellationToken);

            if (existing is null)
            {
                throw new AppException(ResponseMessageExtensions.Transaction.TransactionNotFound);
            }

            var updateResult = await _transactionRepository.UpdateTransaction(id, input);

            if (updateResult.IsError)
            {
                throw new AppException(ResponseMessageExtensions.Database.UpdateFailed);
            }

            return this.OkResponse(ResponseMessageExtensions.Database.UpdateSuccess);
        }
    }
}
