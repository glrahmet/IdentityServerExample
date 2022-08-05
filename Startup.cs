using IdentityServerExample.CustomValidations;
using IdentityServerExample.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServerExample
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

            //appsetting de connection string bilgilerini okuduk ve context inject ettik 
            services.AddDbContext<IdentityDbContextManager>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"));
            });
                        
            //identity sýnýfýný inject ettik.
            //password kýsýmlarý için gerekli kontrolleri kýsýtladýk þifre oluþtururken 
            services.AddIdentity<User, UserRole>(opt =>
            {
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;

                //email uniq karakterler için ve karakter setini belirtmek için kullanýlýr
                opt.User.RequireUniqueEmail = true;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._";

            })                
                .AddPasswordValidator<CustomPasswordValidator>()
                .AddUserValidator<CustomUserValidator>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<IdentityDbContextManager>()             
                .AddDefaultTokenProviders();

            services.AddControllersWithViews();


            //cookies create            
            CookieBuilder cookieBuilder = new CookieBuilder();

            cookieBuilder.Name = "CookiesAamet";
            cookieBuilder.HttpOnly = false;
            cookieBuilder.SameSite = SameSiteMode.Lax;
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;

            services.ConfigureApplicationCookie(opts =>
            {
                opts.LoginPath = new PathString("/Home/Login");
                opts.LogoutPath = new PathString("/Member/LogOut");
                opts.Cookie = cookieBuilder;
                opts.SlidingExpiration = true;
                opts.ExpireTimeSpan = System.TimeSpan.FromDays(60);
                opts.AccessDeniedPath = new PathString("/Member/AccessDenied");
            });

            //admin yetkisi için hatasýný gidermek 
            //The AuthorizationPolicy named: 'Admin' was not found

            services.AddAuthorization(o =>
            {
                o.AddPolicy("FirstStepCompleted", policy => policy.RequireClaim("FirstStepCompleted"));
                o.AddPolicy("Authorized", policy => policy.RequireClaim("Authorized"));
            });

            //services.AddAuthorization(options =>
            //{

            //    options.AddPolicy("Super",
            //        authBuilder =>
            //        {
            //            authBuilder.RequireRole("Administrators");
            //        });

            //});
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            //app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
