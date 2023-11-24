using CustomLibrary.Helper;

namespace BackendService.Data.Domain
{
    public class MsProduct:EntityBase
    {
        public MsProduct()
        {
            MsProductVariants = new HashSet<MsProductVariant>();
        }
        public Guid? MsProductCategoryId { get; set; }
        public string? Plu { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }

        public virtual ICollection<MsProductVariant> MsProductVariants { get; set; }
        public virtual MsProductCategory? MsProductCategory { get; set; }
    }
}
