using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Project.Extensions.Extensions;
using System.IO;

namespace Project.DigitalMarket.Infrastructure.MsSql.Data
{
    public class DigitalMarketDbContextFactory : IDesignTimeDbContextFactory<DigitalMarketDbContext>
    {
        public DigitalMarketDbContext CreateDbContext(string[] args)
        {
            var rootDir = Directory.GetCurrentDirectory();
            
            // Nếu đang đứng ở thư mục con, trỏ lên thư mục gốc
            if (rootDir.Contains("Infrastructure") || rootDir.Contains("Project.DigitalMarket.Infrastructure.MsSql"))
            {
               rootDir = Path.GetFullPath(Path.Combine(rootDir, "..", ".."));
            }

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(rootDir)
                .AddJsonFile("Config/connection.json", optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<DigitalMarketDbContext>();
            
            var connectionString = configuration.GetSection("ConnectionString:SqlServer").Value;

            if (connectionString.IsNullOrEmpty())
            {
                connectionString = "Server=(localdb)\\mssqllocaldb;Database=DigitalMarketDb;Trusted_Connection=True;MultipleActiveResultSets=true";
            }

            builder.UseSqlServer(connectionString);

            return new DigitalMarketDbContext(builder.Options);
        }
    }
}
