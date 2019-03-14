using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TTMMC.Services;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TTMMC
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
            services.AddMvc();
            services.AddDistributedMemoryCache();
            services.AddAntiforgery();

            //sessione 15 giorni
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromHours(360);
                options.Cookie.Name = "TTMMC.Session";
            });

            //database
            services.AddDbContext<DBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services
            services.AddSingleton<KeepAlive>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<Utilities>();
            services.AddSingleton<MachinesService>();
            services.AddSingleton<LayoutListener>();
            services.AddSingleton<Barcode>();
            services.AddRouting(options => options.LowercaseUrls = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            RotativaConfiguration.Setup(env);
        }
    }
}
