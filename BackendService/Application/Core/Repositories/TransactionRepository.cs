using BackendService.Application.Core.IRepositories;
using BackendService.Data;
using BackendService.Data.Domain;
using BackendService.Dtos;
using CustomLibrary.Helper;
using CustomLibrary.Services;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Application.Core.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public TransactionRepository(ApplicationDbContext context
            ,IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async ValueTask<ResponseBaseViewModel> SubmitTransaction(TransactionDto transactionDto)
        {
            var response = new ResponseBaseViewModel();
            await using var transaction = _context.Database.BeginTransaction();
            try
            {
                var transactionModel = new Transaction()
                {
                    TransactionNo = TransactionNumber(),
                    TotalAmount = transactionDto.TotalAmount,
                    CreatedDate = DateTime.UtcNow,
                    CreatedUser = _identityService.GetUserId(),
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUser = _identityService.GetUserId(),
                    IsActive = true,
                    IsDelete = false,
                };

                await _context.Transactions.AddAsync(transactionModel);

                var transactionDetailsModel = new TransactionDetail()
                {
                    MsProductVariantId = transactionDto.MsProductVariantId,
                    Price = transactionDto.Price,
                    Qty = transactionDto.Qty,
                    SubTotal = transactionDto.SubTotal,
                    TransactionId = transactionModel.Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedUser = _identityService.GetUserId(),
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUser = _identityService.GetUserId(),
                    IsActive = true,
                    IsDelete = false,
                };

                await _context.TransactionDetails.AddAsync(transactionDetailsModel);

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

        private string TransactionNumber()
        {
            var findCountAllData = _context.Transactions.Count();
            var continuousNumber = (findCountAllData + 1).ToString("D5");

            var newFormat = "TRX" + continuousNumber;
            return newFormat;
        }

        public async ValueTask<ResponseBaseViewModel> UpdateTransaction(string id, TransactionDto transactionDto)
        {
            var response = new ResponseBaseViewModel();
            await using var transaction = _context.Database.BeginTransaction();
            try
            {
                var existingTransaction = await _context.Transactions
                    .FirstOrDefaultAsync(x => x.Id.ToString() == id);

                existingTransaction.TotalAmount = transactionDto.TotalAmount;
                existingTransaction.UpdatedDate = DateTime.UtcNow;
                existingTransaction.UpdatedUser = _identityService.GetUserId();

                _context.Transactions.Update(existingTransaction);

                var existingTransactionDetails = await _context.TransactionDetails
                    .FirstOrDefaultAsync(x => x.TransactionId.ToString() == existingTransaction.Id.ToString());

                existingTransactionDetails.Qty = transactionDto.Qty;
                existingTransactionDetails.SubTotal = transactionDto.SubTotal;

                _context.TransactionDetails.Update(existingTransactionDetails);

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
    }
}
