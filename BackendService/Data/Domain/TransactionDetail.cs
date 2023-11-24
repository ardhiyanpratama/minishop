using CustomLibrary.Helper;

namespace BackendService.Data.Domain
{
    public class TransactionDetail:EntityBase
    {
        public Guid? TransactionId { get; set; }
        public Guid? MsProductVariantId { get; set; }
        public double? Price { get; set; }
        public double? Qty { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }

        public virtual Transaction? Transaction { get; set; }
        public virtual MsProductVariant? MsProductVariant { get; set; }
    }
}
