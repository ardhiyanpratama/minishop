using BackendService.Data;
using CustomLibrary.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BackendService.Infrastructure.Services
{
    public class PrivateUserIdService : IPrivateUserIdService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContext;

        public PrivateUserIdService(IServiceProvider serviceProvider, IHttpContextAccessor httpContext)
        {
            _serviceProvider = serviceProvider;
            _httpContext = httpContext;
        }

        public async Task<string> GetUserId()
        {
            var username = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (username is null || string.IsNullOrWhiteSpace(username.Value))
            {
                return null;
            }

            var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var user = await dbContext.MsUsers.FirstOrDefaultAsync(e => e.Username == username.Value);
            return user is null ? "" : user.Id.ToString();
        }
    }
}
