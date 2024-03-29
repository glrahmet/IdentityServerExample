using IdentityServerExample.CliamProvider;
using IdentityServerExample.CustomValidations;
using IdentityServerExample.Models;
using Microsoft.AspNetCore.Authentication;
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



            //services.AddAuthentication().AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
            //    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            //});

            //identity s�n�f�n� inject ettik.
            //password k�s�mlar� i�in gerekli kontrolleri k�s�tlad�k �ifre olu�tururken 
            services.AddIdentity<User, UserRole>(opt =>
            {
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;

                //email uniq karakterler i�in ve karakter setini belirtmek i�in kullan�l�r
                opt.User.RequireUniqueEmail = true;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._";
/**/
            })
                .AddPasswordValidator<CustomPasswordValidator>()
                .AddUserValidator<CustomUserValidator>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<IdentityDbContextManager>()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews();

            services.AddScoped<IClaimsTransformation, ClaimProvider>();


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

            //claim i�in plociy belirtiyoruz.

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AnkaraPolicy", policy =>
                {
                    policy.RequireClaim("City", "Ankara");
                });
            });

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
