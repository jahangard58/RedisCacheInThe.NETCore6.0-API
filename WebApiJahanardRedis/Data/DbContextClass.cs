using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApiJahanardRedis.Model;

namespace WebApiJahanardRedis.Data
{
    public class DbContextClass : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbContextClass(IConfiguration configuration)
        {
              _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: Sqlopertion =>
        {
            Sqlopertion.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);

            Sqlopertion.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);

        });
        }

        public virtual DbSet<Product>? Products { get; set; }
    }
}
