using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PortailIEPSM
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
            services.AddControllersWithViews();
            services.AddAuthentication()
            .AddCookie("groupe2_admin", option =>
            {
                option.LoginPath = "/Groupe_2/Connexion";
                option.Cookie.Name = "article_session";
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
                endpoints.MapAreaControllerRoute(
                name: "AreaGroupe_1",
                areaName: "Groupe_1",
                pattern: "Groupe_1/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                name: "AreaGroupe_2",
                areaName: "Groupe_2",
                pattern: "Groupe_2/{controller=Accueil}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                name: "AreaGroupe_3",
                areaName: "Groupe_3",
                pattern: "Groupe_3/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                name: "AreaGroupe_4",
                areaName: "Groupe_4",
                pattern: "Groupe_4/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                name: "AreaGroupe_5",
                areaName: "Groupe_5",
                pattern: "Groupe_5/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                name: "AreaGroupe_6",
                areaName: "Groupe_6",
                pattern: "Groupe_6/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
