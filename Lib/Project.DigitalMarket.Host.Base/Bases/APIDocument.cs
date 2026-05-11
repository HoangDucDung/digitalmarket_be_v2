
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Project.DigitalMarket.Host.Base.Bases
{
    public static class APIDocument
    {
        public static IServiceCollection AddAPIDocument(this IServiceCollection services, string filePath, string docName)
        {
            services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{filePath}.xml";
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = $"{docName} Documentation",
                        Version = "v1",
                        Description = $"Tài liệu tích hợp {docName.ToLower()}",
                        Contact = new OpenApiContact()
                        {
                            Name = "DigitalMarket",
                            Email = "digital_market@gmail.com",

                        },
                    });

                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                options.CustomSchemaIds(type => type.FullName);

            });
            return services;
        }

        public static IApplicationBuilder UseAPIDocument(this IApplicationBuilder app, string docName)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", $"{docName} API");
            });

            // Cấu hình ReDoc làm mặc định
            app.UseReDoc(c =>
            {
                c.RoutePrefix = "docs"; // Đặt làm route mặc định
                c.SpecUrl = "../swagger/v1/swagger.json";
                c.DocumentTitle = $"{docName} Documentation";
                c.HideDownloadButton();
            });

            return app;
        }
    }
}
