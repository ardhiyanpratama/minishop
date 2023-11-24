using Microsoft.AspNetCore.Authorization;

namespace CustomLibrary.Authorization
{
    public class RequirePermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; set; }
        public RequirePermissionRequirement(string permission) => Permission = permission;
    }
}
