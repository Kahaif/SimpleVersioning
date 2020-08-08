using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleVersioning.Data;
using SimpleVersioning.Data.Sql;
using SimpleVersioning.Logger;
using System;

namespace SimpleVersioning
{
    public class Startup
    {
        private readonly bool UseInterface;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            UseInterface = Configuration.GetValue<int>("UseInterface") == 1 ;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("SimpleVersioning");
            services.AddSingleton<IStorageRepositoryAsync>(
                new MariaDBStorageRepository(new DbContextOptionsBuilder<MariaDBServerContext>().UseMySql(connectionString).Options)
            );

            /*
            services.AddDbContext<SqlServerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SimpleVersioning")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<SqlServerContext>();
           */
            if (UseInterface)
            {
                services.AddControllersWithViews();
                services.AddRazorPages();
            }
            else
            {
                services.AddControllers();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                if (UseInterface)
                    app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            
            loggerFactory.AddProvider(
                new LoggerProvider(
                    new LoggerConfiguration()
                    {
                        Callback = s => Console.WriteLine(s)
                    }
                    )); 
            
            app.UseHttpsRedirection();

            if (UseInterface)
            {
                app.UseStaticFiles();
            }
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
                if (UseInterface)
                {
                    endpoints.MapRazorPages();
                }
            });
        }
    }
}
