using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Project.Extensions.Extensions;
using System.IO;

namespace Project.DigitalMarket.Infrastructure.MsSql.Data
{
    //public class DigitalMarketDbContextFactory : IDesignTimeDbContextFactory<DigitalMarketDbContext>
    //{
    //    public DigitalMarketDbContext CreateDbContext(string[] args)
    //    {
    //        var currentDir = Directory.GetCurrentDirectory();
    //        var rootDir = currentDir;
            
    //        // Tìm thư mục gốc chứa thư mục Config
    //        while (!Directory.Exists(Path.Combine(rootDir, "Config")) && Path.GetDirectoryName(rootDir) != null)
    //        {
    //            rootDir = Path.GetDirectoryName(rootDir)!;
    //        }

    //        IConfigurationRoot configuration = new ConfigurationBuilder()
    //            .SetBasePath(rootDir)
    //            .AddJsonFile("Config/connection.json", optional: false) // Bỏ optional để báo lỗi nếu không tìm thấy
    //            .Build();

    //        var builder = new DbContextOptionsBuilder<DigitalMarketDbContext>();
            
    //        var connectionStrings = configuration.GetSection("ConnectionString");
    //        var connectionString = connectionStrings["SqlServer"];

    //        if (connectionString.IsNullOrEmpty())
    //        {
    //            connectionString = "Server=(localdb)\\mssqllocaldb;Database=DigitalMarketDb;Trusted_Connection=True;MultipleActiveResultSets=true";
    //        }

    //        builder.UseSqlServer(connectionString);

    //        return new DigitalMarketDbContext(builder.Options);
    //    }
    //}
}
