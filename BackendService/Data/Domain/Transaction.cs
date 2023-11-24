using CustomLibrary.Helper;

namespace BackendService.Data.Domain
{
    public class Transaction:EntityBase
    {
        public Transaction()
        {
            TransactionDetails = new HashSet<TransactionDetail>();
        }
        public string? TransactionNo { get; set; }
        public double? TotalAmount { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }

        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
    }
}
