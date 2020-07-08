using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskList.Data.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using Microsoft.AspNetCore.Identity;

namespace TaskList.Web
{
    public class Startup
    {
        private static readonly string CustomApiScheme = "CustomApiScheme";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAuthentication(services);                       

            Configuration.GetSection("AppSettings").Bind(AppSettings.Default);

            services.AddControllersWithViews();

            services.AddDbContext<DB>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "app/build";
            });

            services.AddHttpClient();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "app";

                if (env.IsDevelopment())
                {
                    // To run the frontend react app with backend:
                    // spa.UseReactDevelopmentServer(npmScript: "start");
                    // To run the frontend react app separately:
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DB>()
                .AddClaimsPrincipalFactory<MyUserClaimsPrincipalFactory>();

            services.AddAuthorization(options =>
            {
                // options.AddPolicy("AdminRole", policy => policy.RequireClaim("AdminRole"));
                options.AddPolicy(
                    "AdminRole",
                    policy => policy.AddRequirements(new IsAdminRequirement("Admin")));
            });

            services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AzureADDefaults.CookieScheme;
                options.DefaultChallengeScheme = CustomApiScheme;
                options.DefaultSignInScheme = AzureADDefaults.CookieScheme;
            })
            .AddAzureAD(options =>
            {
                Configuration.Bind("AzureAd", options);
            })
            .AddScheme<AuthenticationSchemeOptions, CustomApiAuthenticationHandler>(CustomApiScheme, options => { });

            services.Configure<CookieAuthenticationOptions>(AzureADDefaults.CookieScheme, options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None; // SameSiteMode.Lax;
                options.Cookie.MaxAge = new TimeSpan(7, 0, 0, 0);
            });

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.Authority = options.Authority + "/v2.0/";         // Microsoft identity platform
                options.TokenValidationParameters.ValidateIssuer = false; // accept several tenants (here simplified)
            });

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });


            // claims transformation is run after every Authenticate call
            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
        }
    }
}
