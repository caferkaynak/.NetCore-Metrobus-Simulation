using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StationApplication.Common;
using StationApplication.Data;
using StationApplication.Service;
using System.IO;
using StationApplication.Web.Helpers;
using StationApplication.Entity.Entities;
using Microsoft.AspNetCore.Identity;

namespace StationApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration.GetSection("AppSettings").Get<AppSettings>());
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ISmartTicketTypeService, SmartTicketTypeService>();
            services.AddTransient<ISmartTicketService, SmartTicketService>();
            services.AddTransient<IStationSmartTicketService, StationSmartTicketService>();
            services.AddTransient<IRepostService, ReportService>();
            services.AddTransient<IUserService, UserService>();
            services.AddIdentity<User, IdentityRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();
            services.AddEntityFrameworkSqlServer();
           
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            CurrentDirectoryHelpers.SetCurrentDirectory();
            var path = Path.Combine(Directory.GetCurrentDirectory(),"log4net.config");
            loggerFactory.AddLog4Net(path);
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                         name: "MyArea",
                         template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            SeedData.Seed(app);
        }
    }
}
