using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DML;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Models;
using Services;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddScoped<ICRUD<Album>, AlbumService>();
            services.AddScoped<IAzure, AzureService>();
            services.AddDbContext<DataContext>(
                x => x.UseSqlServer(Configuration.GetConnectionString("Sql")));
            services.AddSwaggerGen(
              k => k.SwaggerDoc("v1", new OpenApiInfo
              {
                  Title = "MyApi",
                  Version = " v1",
                  Description = "Des1"
              }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseSwaggerUI(k => k.SwaggerEndpoint("v1/swagger.json", "My Api V1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
