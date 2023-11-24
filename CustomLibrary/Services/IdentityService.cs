using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CustomLibrary.Services
{
    public class IdentityService : IIdentityService
    {
        public static readonly string UserId = "userid";

        private readonly IHttpContextAccessor context;
        private readonly IPrivateUserIdService userService;
        public IdentityService(IServiceScopeFactory serviceScopeFactory)
        {
            var scope = serviceScopeFactory.CreateScope();
            context = scope.ServiceProvider.GetService<IHttpContextAccessor>();
            userService = scope.ServiceProvider.GetService<IPrivateUserIdService>();
        }
        public string? GetUserId()
        {
            if (userService is null)
            {
                return null;
            }

            var task = userService.GetUserId();
            task.Wait();
            return task.Result;
        }

        public string GetUsername()
        {
            var username = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return username?.Value;
        }

        public static class Role
        {
            public const string Administrator = "Administrator";
            public const string Customer = "Customer";
        }
    }
}
