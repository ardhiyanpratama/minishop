using BackendService.Application.Core.IRepositories;
using BackendService.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Application.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly ILogger _logger;
        protected DbSet<T> dbSet;

        public GenericRepository(
            ApplicationDbContext context,
            ILogger logger)
        {
            _context = context;
            _logger = logger;
            dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"{ex.GetType()} Error: {ex.Message}");
                return Enumerable.Empty<T>();
            }
        }

        public async Task<T> GetAsync(long id)
        {
            try
            {
                return await dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"{ex.GetType()} Error: {ex.Message}");
                return null;
            }
        }
    }
}
