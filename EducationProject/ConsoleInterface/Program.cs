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
using Infrastructure.DAL.Repositories;
using EducationProject.BLL.Interfaces;
using Infrastructure.BLL.Services;
using Infrastructure.BLL;
using ConsoleInterface.Interfaces;
using ConsoleInterface.Implementations;
using ConsoleInterface.Implementations.Commands;

namespace ConsoleInterface
{
    class Program
    {
        static IServiceCollection serviceCollection = new ServiceCollection();

        static void ConfigureServices(IServiceCollection services)
        {
            string xmlFileName = ConfigurationManager.AppSettings.Get("XMLFile");
            string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
            int defaultPageSize = Int32.Parse(ConfigurationManager.AppSettings.Get("DefaultPageSize"));


            services.AddSingleton<DbContext, EducationProjectDbContext>(c => new EducationProjectDbContext(connectionString));

            services.AddSingleton<IRepository<CourseDBO>, BaseRepository<CourseDBO>>();
            services.AddSingleton<IRepository<AccountDBO>, BaseRepository<AccountDBO>>();
            services.AddSingleton<IRepository<SkillDBO>, BaseRepository<SkillDBO>>();
            services.AddSingleton<IRepository<BaseMaterialDBO>, BaseRepository<BaseMaterialDBO>>();
            services.AddSingleton<IRepository<CourseSkillDBO>, BaseRepository<CourseSkillDBO>>();
            services.AddSingleton<IRepository<CourseMaterialDBO>, BaseRepository<CourseMaterialDBO>>();
            services.AddSingleton<IRepository<AccountCourseDBO>, BaseRepository<AccountCourseDBO>>();
            services.AddSingleton<IRepository<AccountMaterialDBO>, BaseRepository<AccountMaterialDBO>>();
            services.AddSingleton<IRepository<AccountSkillDBO>, BaseRepository<AccountSkillDBO>>();

            services.AddSingleton<AuthorizationService>();

            services.AddSingleton<ISkillService, SkillService>();
            services.AddSingleton<ICourseService, CourseService>(c => 
                new CourseService(c.GetRequiredService<IRepository<CourseDBO>>(),
                c.GetRequiredService<AuthorizationService>(),
                c.GetRequiredService<IMaterialService>(),
                c.GetRequiredService<ISkillService>(),
                c.GetRequiredService<IRepository<CourseSkillDBO>>(),
                c.GetRequiredService<IRepository<CourseMaterialDBO>>(),
                defaultPageSize));
            services.AddSingleton<IMaterialService, MaterialService>();
            services.AddSingleton<IAccountService, AccountService>();

            services.AddSingleton<ICommandHandler, CommandHandler>();

            services.AddSingleton<ICommand, InvalidCommand>();
            services.AddSingleton<ICommand, CreateAccountCommand>();
            services.AddSingleton<ICommand, LogInCommand>();
            services.AddSingleton<ICommand, LogOutCommand>();
            services.AddSingleton<ICommand, CreateCourseCommand>();
            services.AddSingleton<ICommand, CreateSkillCommand>();
            services.AddSingleton<ICommand, CreateMaterialCommand>();
            services.AddSingleton<ICommand, GetSkillsCommand>(c =>
                new GetSkillsCommand(c.GetRequiredService<ISkillService>(), defaultPageSize));
            services.AddSingleton<ICommand, GetMaterialsCommand>(c =>
                new GetMaterialsCommand(c.GetRequiredService<IMaterialService>(), defaultPageSize));
            services.AddSingleton<ICommand, GetCoursesCommand>(c =>
                new GetCoursesCommand(c.GetRequiredService<ICourseService>(), defaultPageSize));
            services.AddSingleton<ICommand, GetAccountCourses>(c => 
                new GetAccountCourses(c.GetRequiredService<ICourseService>(), defaultPageSize));
            services.AddSingleton<ICommand, AddSkillToCourseCommand>();
            services.AddSingleton<ICommand, AddMaterialToCourseCommand>();
            services.AddSingleton<ICommand, AddCourseToAccountCommand>();
            services.AddSingleton<ICommand, ShowAccountInfoCommand>();
            services.AddSingleton<ICommand, ChangeCourseStateCommand>();
            services.AddSingleton<ICommand, ShowCourseInfoCommand>();
            services.AddSingleton<ICommand, ShowMaterialInfoCommand>();
            services.AddSingleton<ICommand, PassMaterialCommand>();
            services.AddSingleton<ICommand, PassCourseCommand>();
            services.AddSingleton<ICommand, PublishCourseCommand>();


            services.AddSingleton<IConsoleHandler, ConsoleHandler>();
        }

        static void Main(string[] args)
        {
            ConfigureServices(serviceCollection);

            serviceCollection.BuildServiceProvider()
                .GetService<IConsoleHandler>()
                .Run();

            return;
        }
    }
}
