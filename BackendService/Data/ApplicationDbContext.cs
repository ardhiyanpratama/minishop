using BackendService.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Data
{
    public partial class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<MsUser> MsUsers { get; set; }
        public virtual DbSet<MsUserRole> MsUserRoles { get; set; }
        public virtual DbSet<MsProduct> MsProducts { get; set; }
        public virtual DbSet<MsProductCategory> MsProductCategories { get; set; }
        public virtual DbSet<MsProductVariant> MsProductVariants { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            base.OnModelCreating(modelBuilder);

        }
    }
}
