using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Infrastructure.Context;
using HoneyDo.Infrastructure.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using HoneyDo.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;
using System;

namespace HoneyDo.Web
{
#pragma warning disable CS1591
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Honey-Dos API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var domainInfo = Assembly.GetAssembly(typeof(Todo));
                var domainXmlFile = $"{domainInfo.GetName().Name}.xml";
                var domainXmlPath = Path.Combine(AppContext.BaseDirectory, domainXmlFile);
                c.IncludeXmlComments(domainXmlPath);
                var infrastructureInfo = Assembly.GetAssembly(typeof(LoginModel));
                var infrastructureXmlFile = $"{infrastructureInfo.GetName().Name}.xml";
                var infrastructureXmlPath = Path.Combine(AppContext.BaseDirectory, infrastructureXmlFile);
                c.IncludeXmlComments(infrastructureXmlPath);
                var webXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var webXmlPath = Path.Combine(AppContext.BaseDirectory, webXmlFile);
                c.IncludeXmlComments(webXmlPath);
            });

            services.Configure<ContextOptions<HoneyDoContext>>(options =>
            {
                options.ConnectionString = _configuration["HoneyDoContext"];
            });

            services.Configure<LoginOptions>(options =>
            {
                options.Issuer = _configuration["JwtIssuer"];
                options.MillisecondsUntilExpiration = long.Parse(_configuration["JwtExpireMilliseconds"]);
                options.Key = _configuration["JwtKey"];
                options.FirebaseJson = _configuration["FirebaseJson"];
            });

            services.AddDbContext<HoneyDoContext>();

            services.AddScoped<IRepository<Todo>, ContextRepository<Todo, HoneyDoContext>>();
            services.AddScoped<IRepository<Account>, ContextRepository<Account, HoneyDoContext>>();
            services.AddScoped<IRepository<Login>, ContextRepository<Login, HoneyDoContext>>();

            services.AddScoped<IAccountAccessor, AccountAccessor>();
            services.AddScoped<LoginService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["JwtIssuer"],
                        ValidAudience = _configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]))
                    };
                });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Honey-Dos API v1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    // https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/react?view=aspnetcore-2.1&tabs=netcore-cli#run-the-cra-server-independently
                    // The line below separates the SPA and API into two processes so the SPA doesn't have to rebuild every API rebuild - switch when needed
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                    // spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
#pragma warning restore CS1591
}
