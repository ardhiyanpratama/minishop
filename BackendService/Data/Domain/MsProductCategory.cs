using CustomLibrary.Helper;

namespace BackendService.Data.Domain
{
    public class MsProductCategory:EntityBase
    {
        public MsProductCategory()
        {
            MsProducts = new HashSet<MsProduct>();
        }

        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }

        public virtual ICollection<MsProduct> MsProducts { get; set; }
    }
}
