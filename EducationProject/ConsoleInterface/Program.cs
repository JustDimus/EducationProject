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

            services.AddTransient<DbContext, EducationProjectDbContext>(c => new EducationProjectDbContext(connectionString));

            services.AddTransient<IRepository<CourseDBO>, BaseRepository<CourseDBO>>();
            services.AddTransient<IRepository<AccountDBO>, BaseRepository<AccountDBO>>();
            services.AddTransient<IRepository<SkillDBO>, BaseRepository<SkillDBO>>();
            services.AddTransient<IRepository<BaseMaterialDBO>, BaseRepository<BaseMaterialDBO>>();
            services.AddTransient<IRepository<CourseSkillDBO>, BaseRepository<CourseSkillDBO>>();
            services.AddTransient<IRepository<CourseMaterialDBO>, BaseRepository<CourseMaterialDBO>>();
            services.AddTransient<IRepository<AccountCourseDBO>, BaseRepository<AccountCourseDBO>>();
            services.AddTransient<IRepository<AccountMaterialDBO>, BaseRepository<AccountMaterialDBO>>();
            services.AddTransient<IRepository<AccountSkillDBO>, BaseRepository<AccountSkillDBO>>();

            services.AddSingleton<AuthorizationService>();

            services.AddTransient<ISkillService, SkillService>();
            services.AddTransient<ICourseService, CourseService>(c => 
                new CourseService(c.GetRequiredService<IRepository<CourseDBO>>(),
                c.GetRequiredService<AuthorizationService>(),
                c.GetRequiredService<IMaterialService>(),
                c.GetRequiredService<ISkillService>(),
                c.GetRequiredService<IRepository<CourseSkillDBO>>(),
                c.GetRequiredService<IRepository<CourseMaterialDBO>>(),
                defaultPageSize));
            services.AddTransient<IMaterialService, MaterialService>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddSingleton<ICommandHandler, CommandHandler>();

            services.AddTransient<ICommand, InvalidCommand>();
            services.AddTransient<ICommand, CreateAccountCommand>();
            services.AddTransient<ICommand, LogInCommand>();
            services.AddTransient<ICommand, LogOutCommand>();
            services.AddTransient<ICommand, CreateCourseCommand>();
            services.AddTransient<ICommand, CreateSkillCommand>();
            services.AddTransient<ICommand, CreateMaterialCommand>();
            services.AddTransient<ICommand, GetSkillsCommand>(c =>
                new GetSkillsCommand(c.GetRequiredService<ISkillService>(), defaultPageSize));
            services.AddTransient<ICommand, GetMaterialsCommand>(c =>
                new GetMaterialsCommand(c.GetRequiredService<IMaterialService>(), defaultPageSize));
            services.AddTransient<ICommand, GetCoursesCommand>(c =>
                new GetCoursesCommand(c.GetRequiredService<ICourseService>(), defaultPageSize));
            services.AddTransient<ICommand, GetAccountCourses>(c => 
                new GetAccountCourses(c.GetRequiredService<ICourseService>(), defaultPageSize));
            services.AddTransient<ICommand, AddSkillToCourseCommand>();
            services.AddTransient<ICommand, AddMaterialToCourseCommand>();
            services.AddTransient<ICommand, AddCourseToAccountCommand>();
            services.AddTransient<ICommand, ShowAccountInfoCommand>();
            services.AddTransient<ICommand, ChangeCourseStateCommand>();
            services.AddTransient<ICommand, ShowCourseInfoCommand>();
            services.AddTransient<ICommand, ShowMaterialInfoCommand>();
            services.AddTransient<ICommand, PassMaterialCommand>();
            services.AddTransient<ICommand, PassCourseCommand>();
            services.AddTransient<ICommand, PublishCourseCommand>();

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
