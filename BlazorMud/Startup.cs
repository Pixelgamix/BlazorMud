using Autofac;
using BlazorMud.Contracts.Database;
using BlazorMud.Contracts.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Extensions.Logging;
using System.Text;

namespace BlazorMud
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureSecurity(services);

            services.AddRazorPages();
            services.AddServerSideBlazor();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register(_ => new LoggerFactory(new ILoggerProvider[] { new SerilogLoggerProvider() }))
            .As<ILoggerFactory>()
            .SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            var databaseSettings = new DatabaseSettings();
            Configuration.GetSection("Database").Bind(databaseSettings);
            builder.RegisterInstance(databaseSettings).As<DatabaseSettings>();

            var securitySettings = new SecuritySettings();
            Configuration.GetSection("Security").Bind(securitySettings);
            builder.RegisterInstance(securitySettings).As<SecuritySettings>();

            builder.RegisterType<Blazored.LocalStorage.LocalStorageService>()
                .As<Blazored.LocalStorage.ILocalStorageService>();

            builder.RegisterModule<BusinessLogic.AutofacModule>();
            builder.RegisterModule<DataAccess.AutofacModule>();
        }

        private void ConfigureSecurity(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Security:Tokens:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["Security:Tokens:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Security:Tokens:Key"]))
                    };
                });
        }
    }
}
