using Hangfire;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Project.DigitalMarket.Host.Base.Configs;

namespace Digitalmarket.Controller.Hangfire.Configurations
{
    public static class HangfireConfiguration
    {
        public static void ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection(ConfigKeyConstant.HangfireConfig).Get<HangfireConfig>()?.ConnectionString ?? string.Empty;

            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));
            services.AddHangfireServer();
        }

        public static void ConfigureHangfireDashboard(this IApplicationBuilder builder, IConfiguration configuration)
        {
            var hangfireConfig = configuration.GetSection(ConfigKeyConstant.HangfireConfig).Get<HangfireConfig>()?.Authen;

            builder.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = hangfireConfig?["UserName"],
                        Pass = hangfireConfig?["Password"]
                    }
                ]
            });
        }
    }
}
