
using Digitalmarket.Controller.Base.AppCoreFactory;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Project.DigitalMarket.Host.Base.Bases;
using Project.DigitalMarket.Host.Base.Configs;
using System.Reflection;

namespace Digitalmarket.Controller.Business
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // 1. Tải cấu hình từ các file JSON trước (Cần thiết để NLog đọc được giá trị)
                builder.Configuration.AddBaseConfiguration(
                [
                    "auth.json",
                    "Email.json",
                    "connection.json",
                    "elastic.json"
                ]);

                // 2. Cấu hình NLog: Đăng ký RegisterConfigSettings để đọc được từ builder.Configuration
                LogManager.Setup()
                          .SetupExtensions(s => s.RegisterConfigSettings(builder.Configuration))
                          .LoadConfigurationFromFile("nlog.config");

                // 3. Tích hợp NLog vào ASP.NET Core
                builder.Host.UseNLog();

                var docName = "Business";

                // Add services to the container.
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
                });

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddAPIDocument(Assembly.GetExecutingAssembly().GetName().Name ?? "", docName);

                builder.Services.AddLazyloadFactory();
                builder.Services.UseAppAuthenFactory(builder.Configuration);
                builder.Services.UseAppManagerFactory();
                builder.Services.UseAppBussinessFactory();

                builder.Services.GetAuthConfig(builder.Configuration);
                builder.Services.GetEmailConfig(builder.Configuration);
                builder.Services.GetConnectionConfig(builder.Configuration);
                builder.Services.GetElasticConfig(builder.Configuration);

                var app = builder.Build();

                app.UseCors("AllowAll");
                app.UseAPIDocument(docName);
                app.MiddlewareRegistration();
                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                // Log thông báo service đã sẵn sàng bằng Logger mặc định của .NET
                app.Logger.LogInformation("Digitalmarket Business Service is ready and running.");

                app.Run();
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.Fatal(ex, "Service Digitalmarket Business stopped due to an exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
