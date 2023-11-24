using BackendService.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Data.DataSeed
{
    public class SeedData
    {
        public static void Seed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            applicationDbContext.Database.Migrate();

            if (!applicationDbContext.MsUserRoles.Any())
            {
                var userRole = new List<MsUserRole> {
                    new MsUserRole
                    {
                        Name = "Administrator",
                        IsActive= true,
                    },
                    new MsUserRole
                    {
                        Name = "Customer",
                        IsActive= true,
                    },
                };

                applicationDbContext.AddRange(userRole);
                applicationDbContext.SaveChanges();
            }


        }
    }
}
