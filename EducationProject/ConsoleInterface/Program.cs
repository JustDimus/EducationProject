using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using EducationProject.EFCore;
using EducationProject.Core.DAL.EF;
using EducationProject.DAL.Interfaces;
using Infrastructure.DAL.EF.Mappings;
using EducationProject.BLL.Interfaces;
using Infrastructure.BLL.Services;
using Infrastructure.BLL;

namespace ConsoleInterface
{
    class Program
    {
        static IServiceCollection serviceCollection = new ServiceCollection();

        static void ConfigureServices(IServiceCollection services)
        {
            string xmlFileName = ConfigurationManager.AppSettings.Get("XMLFile");
            string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

            services.AddSingleton<DbContext, EducationProjectDbContext>(c => new EducationProjectDbContext(connectionString));

            services.AddSingleton<IRepository<CourseDBO>, BaseRepository<CourseDBO>>();
            services.AddSingleton<IRepository<AccountDBO>, BaseRepository<AccountDBO>>();
            services.AddSingleton<IRepository<SkillDBO>, BaseRepository<SkillDBO>>();
            services.AddSingleton<IRepository<BaseMaterialDBO>, BaseRepository<BaseMaterialDBO>>();
            services.AddSingleton<IRepository<CourseSkillDBO>, BaseRepository<CourseSkillDBO>>();
            services.AddSingleton<IRepository<CourseMaterialDBO>, BaseRepository<CourseMaterialDBO>>();



            services.AddSingleton<AuthorizationService>();

            services.AddSingleton<ISkillService, SkillService>();
            services.AddSingleton<ICourseService, CourseService>();
            services.AddSingleton<IMaterialService, MaterialService>();
            services.AddSingleton<IAccountService, AccountService>();
        }

        static void Main(string[] args)
        {
            ConfigureServices(serviceCollection);

            var provider = serviceCollection.BuildServiceProvider();

            var token = provider.GetService<IAccountService>()
                .LogIn(new EducationProject.BLL.Models.AccountAuthorizationDataDTO()
                {
                    Email = "hello@gmail.com",
                    Password = "user"
                });

            var materials = provider.GetService<IMaterialService>()
                .Get(new EducationProject.BLL.Models.PageInfoDTO()
                {
                    PageNumber = 0,
                    PageSize = 30
                }).ToList();

            var courses = provider.GetService<ICourseService>()
                 .Get(new EducationProject.BLL.Models.PageInfoDTO()
                 {
                     PageNumber = 0,
                     PageSize = 30
                 }).ToList();

            var isAdded = provider.GetService<ICourseService>()
                .ChangeCourseVisibility(new EducationProject.BLL.Models.CourseVisibilityDTO()
                {
                    Token = token,
                    CourseId = courses.First().Id,
                    Visibility = false
                });

            return;

            //ConfigureServices(_services);

            //_services.BuildServiceProvider().GetService<IConsoleHandler>().Run();
        }

    }
}
