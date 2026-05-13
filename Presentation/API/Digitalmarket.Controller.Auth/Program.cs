
using Digitalmarket.Controller.Base.AppCoreFactory;
using Digitalmarket.Controller.Base.Constants;
using Project.DigitalMarket.Application;
using Project.DigitalMarket.Domain;
using Project.DigitalMarket.Host.Base.Bases;
using Project.DigitalMarket.Host.Base.Configs;
using Project.DigitalMarket.Host.Base.Configurations;
using Project.DigitalMarket.Infrastructure.MsSql;
using Project.DigitalMarket.Infrastructure.MsSql.Configurations;
using System.Reflection;

namespace Digitalmarket.Controller.Auth
{
    public class Program
    { 
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddBaseConfiguration(
            [
                ConfigJsonName.Auth,
                ConfigJsonName.Email,
                ConfigJsonName.Connection,
                ConfigJsonName.Elastic
            ]);


            var docName = RedocName.Auth;

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

            // Cấu hình JWT Authentication
            JwtConfiguration.ConfigureJwt(builder.Services, builder.Configuration);

            // Đăng ký các service tùy chỉnh
            builder.Services.AddLazyloadFactory();
            builder.Services.AddAuthServiceFactory();
            builder.Services.AddAuthDomainFactory();
            builder.Services.AddAuthMsSqlFactory();

            // Đăng ký các options
            builder.Services.GetAuthConfig(builder.Configuration);
            builder.Services.GetEmailConfig(builder.Configuration);
            builder.Services.GetConnectionConfig(builder.Configuration);
            builder.Services.GetElasticConfig(builder.Configuration);

            // Đăng ký AutoMapper
            builder.Services.AddAutoMapper(typeof(DigitalMarketAutoMapper));

            // Đăng ký cấu hình Identity
            builder.Services.AddMsSqlIdentity(builder.Configuration);

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

