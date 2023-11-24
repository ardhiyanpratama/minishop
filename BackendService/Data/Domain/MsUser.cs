using CustomLibrary.Helper;

namespace BackendService.Data.Domain
{
    public class MsUser:EntityBase
    {
        public string? Fullname { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? IdCardNumber { get; set; }
        public string? Address { get; set; }
        public string? PlaceOfBirth { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Sex { get; set; }
        public string? MaritalStatus { get; set; }
        public string? ProfilePicture { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Nationality { get; set; }
        public string? Email { get; set; }
        public DateTimeOffset? JoinedAt { get; set; }
        public DateTime? Lastlogin { get; set; }
        public Guid MsUserRoleId { get; set; }

        public virtual MsUserRole? MsUserRole { get; set; }
    }
}
