using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductCatalogAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var databaseServer = Configuration["DatabaseServer"];
            var databaseName = Configuration["DatabaseName"];
            var databaseUser = Configuration["DatabaseUser"];
            var databasePassword = Configuration["DatabasePassword"];
            var connectionString = $"Server={databaseServer};Database={databaseName};User Id={databaseUser};Password={databasePassword}";
            services.AddDbContext<CatalogContext>(options =>
                                                      options.UseSqlServer(connectionString));

            services.AddSwaggerGen(options =>  //going to generate documentation
            {
                options.SwaggerDoc("File1", new Microsoft.OpenApi.Models.OpenApiInfo //"File1" is name for documentation, can be different name than version name v1
                {
                    Title = "Jewelsoncontainers - Product catalog API",
                    Version = "v1",
                    Description = "Product catalog microservice"
                });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger()
               .UseSwaggerUI(e =>
               {
                   e.SwaggerEndpoint("/swagger/File1/swagger.json", "ProductCatalogAPI V1"); //File1 is name of documentation file, so it should match exactly;
                                                                                             //location to get swagger is domain name/swagger/nameofthefile/swagger.json
                                                                                             //ProductCatalogAPI V1 is the title of the page
               });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
