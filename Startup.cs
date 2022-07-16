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


            //cookies create
            CookieBuilder cookieBuilder = new CookieBuilder();
            cookieBuilder.Name = "MyAccount";

            //client side taraf�nda cookie bilgilerine eri�imemesi i�in 
            cookieBuilder.HttpOnly = false;
            //cookie s�resi 
            cookieBuilder.Expiration = System.TimeSpan.FromHours(1);


            //lax default olarak geliyor 
            //
            cookieBuilder.SameSite = SameSiteMode.Lax;

            //strict sadece benim sitem �zerinden gelenleri tut bankac�l�k uygulamalar�nda 
            //cookieBuilder.SameSite = SameSiteMode.Strict;

            //https �zerinden sadece g�nderir
            //cookieBuilder.SecurePolicy = CookieSecurePolicy.Always;

            //http ve https �zerinden geldi�i �ekilde g�nderir 
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //hi� bir �ey yapmaz 
            //cookieBuilder.SecurePolicy = CookieSecurePolicy.None;



            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = new PathString("/Home/Login");
                opt.LogoutPath = new PathString("/Home/Logout");
                opt.Cookie = cookieBuilder;
                //son g�n�nde ya da saatinde tekrardan istek yapar ise cookie bilgisinin s�resini otomatik olarak uzat�r.
                opt.SlidingExpiration = true;
            });


            //identity s�n�f�n� inject ettik.
            //password k�s�mlar� i�in gerekli kontrolleri k�s�tlad�k �ifre olu�tururken 
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;

                //email uniq karakterler i�in ve karakter setini belirtmek i�in kullan�l�r
                opt.User.RequireUniqueEmail = true;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._";

            })
                .AddPasswordValidator<CustomPasswordValidator>()
                .AddUserValidator<CustomUserValidator>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<IdentityDbContextManager>();

            services.AddControllersWithViews();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
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
