using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Project.DigitalMarket.Domain.Share.Config;
using Project.DigitalMarket.Libs.Exceptions;
using Project.DigitalMarket.Libs.Constants.ErrorCode;

namespace Project.DigitalMarket.Host.Base.Configs
{
    public static class AppGetSectionConfigure
    {
        public static void GetSectionConfigure<TOptions>(this IServiceCollection services, IConfiguration configuration, string sectionName) where TOptions : class
        {
            var section = configuration.GetSection(sectionName);

            if (!section.Exists())
                throw new BusinessException(ErrorCode.ConfigSectionNotFound, $"Section {sectionName} không tồn tại trong cấu hình.");

            services.Configure<TOptions>(section);
        }

        public static void GetAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            GetSectionConfigure<AuthConfig>(services, configuration, ConfigKeyConstant.AuthConfig);
            services.AddSingleton<IAuthConfig>(sp => sp.GetRequiredService<IOptions<AuthConfig>>().Value);
        }

        public static void GetEmailConfig(this IServiceCollection services, IConfiguration configuration)
        {
            GetSectionConfigure<EmailConfig>(services, configuration, ConfigKeyConstant.EmailConfig);
            services.AddSingleton<IEmailConfig>(sp => sp.GetRequiredService<IOptions<EmailConfig>>().Value);
        }

        public static void GetConnectionConfig(this IServiceCollection services, IConfiguration configuration)
        {
            GetSectionConfigure<ConnectionString>(services, configuration, ConfigKeyConstant.ConnectionString);
        }

        public static void GetElasticConfig(this IServiceCollection services, IConfiguration configuration)
        {
            GetSectionConfigure<ElasticConfig>(services, configuration, ConfigKeyConstant.ElasticConfig);
        }

        public static void GetHangfireConfig(this IServiceCollection services, IConfiguration configuration)
        {
            GetSectionConfigure<HangfireConfig>(services, configuration, ConfigKeyConstant.HangfireConfig);
        }


        #region kafka
        public static void GetKafkaConfig(this IServiceCollection services, IConfiguration configuration)
        {
            GetSectionConfigure<KafkaConfig>(services, configuration, ConfigKeyConstant.TestNumberOneKafka);
        }

        public static void GetProducerCommonConfig(this IServiceCollection services, IConfiguration configuration)
        {
            GetSectionConfigure<ProducerCustomConfig>(services, configuration, ConfigKeyConstant.ProducerCommon);
        }
        #endregion
    }
}
