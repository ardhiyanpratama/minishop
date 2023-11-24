using CustomLibrary.Adapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace CustomLibrary.Authorization
{
    public class RequirePermissionAuthorizationHandler : AuthorizationHandler<RequirePermissionRequirement>
    {
        private readonly ILoggerAdapter<RequirePermissionAuthorizationHandler> _logger;

        public RequirePermissionAuthorizationHandler(ILogger<RequirePermissionAuthorizationHandler> logger)
        {
            _logger = new LoggerAdapter<RequirePermissionAuthorizationHandler>(logger);
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequirePermissionRequirement requirement)
        {
            _logger.LogInformation("evaluating authorization requirement for permission {0}", requirement.Permission);
            var roleClaim = context.User.FindFirst(ClaimTypes.Role);
            var roleIdClaim = context.User.FindFirst("role");

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
