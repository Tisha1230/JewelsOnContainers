using CartApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi
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
            services.AddControllers().AddNewtonsoftJson();
            services.AddTransient<ICartRepository, RedisCartRepository>();
            services.AddSingleton<ConnectionMultiplexer>(cm =>
            {
                var configuration = ConfigurationOptions.Parse(Configuration["ConnectionString"], true); //just take the part of connectionstring that makes sense to ReDis
                configuration.ResolveDns = true; //sometimes a machine can be given a Domain name. ResolveDns means convert the domain name into the IP address
                configuration.AbortOnConnectFail = false; //if connection fails sometimes, don't abort
                return ConnectionMultiplexer.Connect(configuration); //connect using configuration to the VM(docker container in this case)and return it back to the injection
            });

            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); //clearing any old claims in token handler 

            var identityUrl = Configuration["IdentityUrl"];
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {

                options.Authority = identityUrl.ToString();
                options.RequireHttpsMetadata = false;
                options.Audience = "basket"; //name it is going to be recognised by. it is mentioned in TokenApi>config
            });


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("File1", new OpenApiInfo
                {
                    Title = "Basket HTTP API",
                    Version = "v1",
                    Description = "The Basket Service HTTP API"
                });
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2, //OAuth2: industrialized authentication, ask for username and password or Api name and secret code
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{Configuration.GetValue<string>("IdentityUrl")}/connect/authorize", UriKind.Absolute), //IdentityUrls tells where the tokenservice is located
                            Scopes = new Dictionary<string, string>
                            {
                                 { "basket", "Basket Api" }
                            }
                        }
                    }
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

            app.UseAuthentication(); 

            app.UseAuthorization();

            app.UseSwagger()
              .UseSwaggerUI(e =>
              {
                  e.SwaggerEndpoint("/swagger/File1/swagger.json", "BasketAPI V1"); //File1 is name of documentation file, so it should match exactly;
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
