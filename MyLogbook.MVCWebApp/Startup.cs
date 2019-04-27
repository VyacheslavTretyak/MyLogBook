using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyLogbook.AppContext;
using MyLogbook.Entities;
using MyLogbook.Repositories;

namespace MyLogbook.MVCWebApp
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
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MyConnectionString1")));

			services.AddDefaultIdentity<User>()
				.AddDefaultUI(UIFramework.Bootstrap4)
				.AddEntityFrameworkStores<AppDbContext>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IFacultyRepository, FacultyRepositoty>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<ITeacherRepository, TeacherRepository>();
            services.AddTransient<ISubjectRepository, SubjectRepository>();
            services.AddTransient<IMarkRepository, MarkRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
            else
            {
                app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

			app.UseHttpsRedirection();
			app.UseStaticFiles();
            app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
    }
}
