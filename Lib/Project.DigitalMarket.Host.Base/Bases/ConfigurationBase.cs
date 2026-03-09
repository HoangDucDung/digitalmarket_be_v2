using Microsoft.Extensions.Configuration;
using Project.Extensions.Extensions;

namespace Project.DigitalMarket.Host.Base.Bases
{
    public static class ConfigurationBase
    {
        // Phương thức này giờ sẽ trả về IConfigurationBuilder
        public static IConfigurationBuilder AddBaseConfiguration(this IConfigurationBuilder builder, List<string> fileConfigs)
        {
            if (fileConfigs.IsNullOrEmpty()) return builder;

            var pathConfigs = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Config");

            builder.SetBasePath(pathConfigs);

            foreach (var file in fileConfigs)
            {
                builder.AddJsonFile(file, optional: true, reloadOnChange: true);
            }

            return builder;
        }
    }
}
