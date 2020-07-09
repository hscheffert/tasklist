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
using TaskList.Business.Helpers;
using System.Security.Claims;
using System.Collections.Generic;

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

            services.AddDbContext<DB>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

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
            services.AddAuthorization(options =>
            {
                // options.AddPolicy("Admin", policy => policy.RequireClaim(AppSettings.Default.AdminClaim));
                options.AddPolicy(
                    "Admin",
                    policy => policy.AddRequirements(new HasClaimRequirement(AppSettings.Default.AdminRoleClaim)));
            });

            services.AddSingleton<IAuthorizationHandler, HasClaimHandler>();

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
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.MaxAge = new TimeSpan(7, 0, 0, 0);
                options.Events = new CookieAuthenticationEvents() {};
            });
            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.Events = new OpenIdConnectEvents
                {
                    // Invoked when an IdToken has been validated and produced an AuthenticationTicket.
                    // AKA when the user logs in with Azure AD
                    OnTokenValidated = async (ctx) =>
                    {
                        var email = ctx.Principal.FindFirst(ClaimTypes.Email);
                        var user = Users.GetByEmail(email?.Value);

                        // Could also do ClaimTypes.Role and set "Admin" or "User"
                        // Add the AdminRole claim if they are a supervisor
                        if (user != null && user.IsSupervisor)
                        {
                            var claims = new List<Claim> { new Claim(AppSettings.Default.AdminRoleClaim, "true") };
                            var appIdentity = new ClaimsIdentity(claims);
                            ctx.Principal.AddIdentity(appIdentity);
                        }
                    },
                };
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
        }
    }
}
