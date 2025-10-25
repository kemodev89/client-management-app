using System; // for Version
using ClientDashboardAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // ServerType etc.

namespace ClientDashboardAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Connection string from appsettings.Development.json
            var cs = Configuration.GetConnectionString("DefaultConnection");

            // EF Core 3.1 + Pomelo 3.2 style registration
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseMySql(cs, mySqlOptions =>
                    mySqlOptions
                        .ServerVersion(new Version(8, 0, 22), ServerType.MySql) // matches mysql:8.0.22 in docker-compose
                        .EnableRetryOnFailure()
                )
            );

            // CORS for Vue dev server
            services.AddCors(o => o.AddDefaultPolicy(p =>
                p.WithOrigins("http://localhost:8080")
                 .AllowAnyHeader()
                 .AllowAnyMethod()));

            services.AddControllers();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ClientDashboardAPI - WebApi",
                });
            });
        }

        // Configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClientDashboardAPI - v1");
            });

            app.UseRouting();

            // Apply default CORS policy
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
