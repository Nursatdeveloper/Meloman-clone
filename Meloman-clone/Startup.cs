using Meloman_clone.Data;
using Meloman_clone.Repository;
using Meloman_clone.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Meloman_clone
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
            services.AddControllersWithViews()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    o.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            services.AddDbContext<BookContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PostgresConnection")));
            services.AddDbContext<UserContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PostgresConnection")));
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PostgresConnection")));

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options => //CookieAuthenticationOptions
                                {
                                    options.LoginPath = "/User/Login";
                    });
            services.AddAuthentication("MelomanAuthCookie").AddCookie("MelomanAuthCookie", options =>
            {
                options.Cookie.Name = "MelomanAuthCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/User/Login";
            });

            services.AddAuthentication("MelomanAdminCookie").AddCookie("MelomanAdminCookie", options =>
            {
                options.Cookie.Name = "MelomanAdminCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/User/Login";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyUsers", policy => policy.RequireClaim("Account", "Exist"));
                options.AddPolicy("OnlyAdmins", policy => policy.RequireClaim("IsAdmin", "True"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
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
