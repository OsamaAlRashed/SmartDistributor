using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntime;
using Microsoft.OpenApi.Models;
using SmartDistributor.Main.Data;
using SmartDistributor.Main.Idata;
using SmartDistributor.SqlServer.Database;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api
{
    public class Startup
    {
        const string ModelPath = @"\Models\pasengers.onnx";
        private readonly IWebHostEnvironment host;

        public Startup(IConfiguration configuration , IWebHostEnvironment host)
        {
            Configuration = configuration;
            this.host = host;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var FullPath = Path.Combine(host.WebRootPath, "Models\\pasengers.onnx");
            //var FullPath = @"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\Models\pasengers.onnx";
            services.AddControllers();
            services.AddDbContext<SmartDistributorDbContext>
              (options =>
              {
                  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); // DefaultConnection
              });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1.0", new OpenApiInfo()
                {
                    Title = "Main API v1.0,",
                    Version = "v1.0"
                });

                options.CustomSchemaIds(x => x.FullName);

            });
            services.AddScoped<IMainRepository, MainRepository>();
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<IMLRepository, MLRepository>();
            services.AddScoped<IApiRepository, ApiRepository>();
            services.AddSingleton<InferenceSession>(
                new InferenceSession(FullPath)
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Versioned API v1.0");
                c.DocExpansion(DocExpansion.None);
            });

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
