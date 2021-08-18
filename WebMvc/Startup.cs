using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Infrastructure;
using WebMvc.Models;
using WebMvc.Services;

namespace WebMvc
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
            //services.AddControllersWithViews();
            //services.AddSingleton<IHttpClient, CustomHttpClient>(); //it will automatically hook up implementation of IHttpClient with CustomHttpClient. no need to instantiate
            //                                                        //Singleton: design pattern. Single Instance of Class.
            //                                                        //At any point in time there will be only one instance of HttpClient. i.e. startup will create a single instance of CustomHttpClient and pass along to anywhere that is going to ask for IHttpClient
            //services.AddTransient<ICatalogService, CatalogService>(); //Transient: Allow as many instances as necessary of this. cuz we don't want to block the data
            //services.AddTransient<IIdentityService<ApplicationUser>, IdentityService>();


            //var identityUrl = Configuration.GetValue<string>("IdentityUrl"); //where the token service is located, localhost:7800.
            //                                                                 //Here is where the token is authorized,generate a token
            //var callBackUrl = Configuration.GetValue<string>("CallBackUrl");//CallBackUrl: once the service is done, it needs to know where to come back to, i.e callbackurl.
            //                                                                //this will be MVC project itself.
            //                                                                //Here is where it should comeback when its done

            //services.AddAuthentication(options => //setting up Authentication for project
            //{
            //    options.DefaultScheme = "Cookies"; //tokens will be carried as cookies; cookies meaning as a part of the request 
            //    options.DefaultChallengeScheme = "oidc"; //challenge isasking for login and password is through using connect

            //    //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    //options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //    //options.DefaultAuthenticateScheme = "Cookies";
            //})
            //.AddCookie("Cookies")
            //.AddOpenIdConnect("oidc", options => //oidc scheme is provided by the following
            //{
            //    options.SignInScheme = "Cookies";
            //    //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            //    options.Authority = identityUrl.ToString(); //this is url to token service
            //    options.SignedOutRedirectUri = callBackUrl.ToString();
            //    options.ClientId = "mvc";   //this should match that client in TokenService
            //    options.ClientSecret = "secret";  //this should match that secret in TokenService
            //    options.ResponseType = "code id_token";
            //    options.SaveTokens = true;
            //    options.GetClaimsFromUserInfoEndpoint = true;
            //    options.RequireHttpsMetadata = false;
            //    options.Scope.Add("openid");
            //    options.Scope.Add("profile");
            //    options.Scope.Add("offline_access");
            //    options.Scope.Add("basket");
            //    options.Scope.Add("order");
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {

            //        NameClaimType = "name",
            //        RoleClaimType = "role"
            //    };



            //});

            services.AddControllersWithViews();
            services.AddSingleton<IHttpClient, CustomHttpClient>();
            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<IIdentityService<ApplicationUser>, IdentityService>();

            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            var callBackUrl = Configuration.GetValue<string>("CallBackUrl");
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";

                //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                //options.DefaultAuthenticateScheme = "Cookies";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.Authority = identityUrl.ToString();
                options.SignedOutRedirectUri = callBackUrl.ToString();
                options.ClientId = "mvc";
                options.ClientSecret = "secret";
                options.ResponseType = "code id_token";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.RequireHttpsMetadata = false;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("offline_access");
                options.Scope.Add("basket");
                options.Scope.Add("order");
                options.TokenValidationParameters = new TokenValidationParameters()
                {

                    NameClaimType = "name",
                    RoleClaimType = "role"
                };



            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Catalog}/{action=Index}");
            });
        }
    }
}
