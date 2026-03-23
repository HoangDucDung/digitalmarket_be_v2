
using Digitalmarket.Controller.Base.AppCoreFactory;
using Project.DigitalMarket.Host.Base.Bases;
using Project.DigitalMarket.Host.Base.Configs;
using System.Reflection;

namespace Digitalmarket.Controller.Business
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddBaseConfiguration(
            [
                "auth.json",
                "Email.json",
                "connection.json"
            ]);


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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            // Thêm dịch vụ tạo tài liệu API
            builder.Services.AddAPIDocument(Assembly.GetExecutingAssembly().GetName().Name ?? "", docName);

            // Đăng ký các dịch vụ tùy chỉnh
            builder.Services.AddLazyloadFactory();
            builder.Services.UseAppAuthenFactory(builder.Configuration);
            builder.Services.UseAppManagerFactory();
            builder.Services.UseAppBussinessFactory();

            // Đăng ký các options
            builder.Services.GetAuthConfig(builder.Configuration);
            builder.Services.GetEmailConfig(builder.Configuration);
            builder.Services.GetConnectionConfig(builder.Configuration);

            var app = builder.Build();

            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            app.UseAPIDocument(docName);

            // Sử dụng middleware tùy chỉnh
            app.MiddlewareRegistration();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
