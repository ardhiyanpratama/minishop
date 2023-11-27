using BackendService.Data.Domain;
using BackendService.Dtos;
using CustomLibrary.Helper;

namespace BackendService.Application.Core.IRepositories
{
    public interface ITransactionRepository
    {
        ValueTask<ResponseBaseViewModel> SubmitTransaction(TransactionDto transactionDto);
        ValueTask<ResponseBaseViewModel> UpdateTransaction(string id, TransactionDto transactionDto);
    }
}
