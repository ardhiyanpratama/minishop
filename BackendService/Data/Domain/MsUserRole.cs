using CustomLibrary.Helper;

namespace BackendService.Data.Domain
{
    public class MsUserRole:EntityBase
    {
        public MsUserRole()
        {
            MsUsers = new HashSet<MsUser>();
        }

        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }

        public virtual ICollection<MsUser> MsUsers { get; set; }
    }
}
