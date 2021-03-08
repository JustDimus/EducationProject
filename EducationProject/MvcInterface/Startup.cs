using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationProject.DAL.Interfaces;
using EducationProject.EFCore;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcInterface.Models.Validators;
using Infrastructure.DAL.Repositories;
using EducationProject.BLL.Interfaces;
using EducationProject.Infrastructure.BLL.Services;
using MvcInterface.ServiceResultController.Interfaces;
using MvcInterface.ServiceResultController.Implementations;
using EducationProject.BLL;
using EducationProject.Core.Models;
using EducationProject.Infrastructure.BLL.Mappings;
using EducationProject.Infrastructure.BLL.PasswordHasher;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MvcInterface
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
            services.AddDbContext<EducationProjectDbContext>(
                options => options.UseSqlServer(
                    this.Configuration.GetConnectionString("DefaultDbConnection")));

            services.AddHttpContextAccessor();

            services.AddScoped<DbContext>(
                c => c.GetService<EducationProjectDbContext>());

            services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ICourseService, CourseService>();

            services.AddTransient<ISkillService, SkillService>();

            services.AddTransient<ServiceResultMessageCollection>();

            services.AddTransient<AccountMapping>();            
            services.AddTransient<CourseMapping>();       
            services.AddTransient<MaterialMapping>();
            services.AddTransient<SkillMapping>();

            services.AddTransient<IMaterialService, MaterialService>();

            services.AddTransient<IServiceResultParser, ServiceResultParser>();

            services.AddTransient<IPasswordHasher, PasswordHasher>();

            services.AddControllersWithViews(setup =>
            {
            }).AddFluentValidation(
                conf => conf.RegisterValidatorsFromAssemblyContaining<RegistrationValidator>());

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/LogIn");
                    options.Cookie.Name = "EducationCookie";
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
