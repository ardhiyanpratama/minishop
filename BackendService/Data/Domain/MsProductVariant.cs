using CustomLibrary.Helper;

namespace BackendService.Data.Domain
{
    public class MsProductVariant:EntityBase
    {
        public MsProductVariant()
        {
            TransactionDetails = new HashSet<TransactionDetail>();
        }
        public Guid? MsProductId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public double? Qty { get; set; }
        public double? Price { get; set; }
        public string? ImageLocation { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }

        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
        public virtual MsProduct? MsProduct { get; set; }
    }
}
