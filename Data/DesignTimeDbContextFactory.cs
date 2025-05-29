using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace RecipeApi.Data
{
    /// This factory is used by EF Core tools at design time
    /// to create your ApplicationDbContext with the correct
    /// connection string and provider.
    // duoc su dung khi tao migration
    //  giup Entity Framework Core biet cach ket noi toi database

    public class DesignTimeDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            try
            {
                // Tao configuration tu appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                // Lay connection string tu configuration
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json");
                }

                // Cau hinh DbContext voi SQL Server
                var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
                builder.UseSqlServer(connectionString);

                // Tra ve instance cua ApplicationDbContext
                return new ApplicationDbContext(builder.Options);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creating ApplicationDbContext", ex);
            }
        }
    }
}
