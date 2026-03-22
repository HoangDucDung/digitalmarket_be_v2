using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Project.DigitalMarket.Infrastructure.MsSql.Data
{
    public class DigitalMarketDbContextFactory : IDesignTimeDbContextFactory<DigitalMarketDbContext>
    {
        public DigitalMarketDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Presentation/API/Digitalmarket.Controller.Auth/appsettings.json", optional: true)
                .AddJsonFile("Presentation/API/Digitalmarket.Controller.Auth/connection.json", optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<DigitalMarketDbContext>();
            // Lấy kết nối từ connection.json nếu có, nếu không thì hardcode tạm để migrate
            var connectionString = configuration.GetSection("ConnectionString:SqlServer").Value 
                                   ?? "Server=(localdb)\\mssqllocaldb;Database=DigitalMarketDb;Trusted_Connection=True;MultipleActiveResultSets=true";

            builder.UseSqlServer(connectionString);

            return new DigitalMarketDbContext(builder.Options);
        }
    }
}
