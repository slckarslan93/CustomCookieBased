using CustomCookieBased.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCookieBased
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CookieContext>(opt =>
            {
                opt.UseSqlServer("server=.; database=CookieDb; integrated security=true;");
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
            {
                opt.Cookie.Name = "CustomCookie";
                opt.Cookie.HttpOnly = true;
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.ExpireTimeSpan=TimeSpan.FromDays(10);
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication(); //routingin altt�na endpointsin �st�ne gelecek �ekilde konumland�rlmas�n.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
