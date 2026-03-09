using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.DigitalMarket.Libs.Exceptions;

namespace Project.DigitalMarket.Host.Base.Configs
{
    public static class AppGetSectionConfigure
    {
        public static void GetSectionConfigure<TOptions>(this IServiceCollection services, IConfiguration configuration, string sectionName) where TOptions : class
        {
            var section = configuration.GetSection(sectionName);

            if (!section.Exists())
                throw new BusinessException($"Section {sectionName} không tồn tại trong cấu hình.");

            services.Configure<TOptions>(section);
        }

        public static void GetAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            GetSectionConfigure<AuthConfig>(services, configuration, "AuthConfig");
        }

        public static void GetConnectionConfig(this IServiceCollection services, IConfiguration configuration)
        {
            GetSectionConfigure<ConnectionString>(services, configuration, "ConnectionString");
        }

        public static void GetKafkaConfig(this IServiceCollection services, IConfiguration configuration)
        {
            GetSectionConfigure<KafkaConfig>(services, configuration, "TestNumberOneKafka");
        }

        public static void GetProducerCommonConfig(this IServiceCollection services, IConfiguration configuration)
        {
            GetSectionConfigure<ProducerCustomConfig>(services, configuration, "ProducerCommon");
        }

        public static void GetConsumerTestKafkaConfig(this IServiceCollection services, IConfiguration configuration)
        {
            GetSectionConfigure<ConsumerConfig>(services, configuration, "TestKafka");
        }
    }
}
