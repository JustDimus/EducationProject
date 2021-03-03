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
            var values = ConfigurationManager.AppSettings.GetValues("CommandName");

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


            services.AddTransient<ICommand, InvalidCommand>(c => 
                new InvalidCommand(ConfigurationManager.AppSettings.Get("InvalidCommand")));
            services.AddTransient<ICommand, CreateAccountCommand>(c =>
                new CreateAccountCommand(c.GetRequiredService<IAccountService>(),
                ConfigurationManager.AppSettings.Get("CreateAccountCommand")));
            services.AddTransient<ICommand, LogInCommand>(c => 
                new LogInCommand(c.GetRequiredService<IAccountService>(),
                ConfigurationManager.AppSettings.Get("LoginCommand")));
            services.AddTransient<ICommand, LogOutCommand>(c =>
                new LogOutCommand(c.GetRequiredService<IAccountService>(),
                ConfigurationManager.AppSettings.Get("LogoutCommand")));
            services.AddTransient<ICommand, CreateCourseCommand>(c => 
                new CreateCourseCommand(c.GetRequiredService<ICourseService>(),
                ConfigurationManager.AppSettings.Get("CreateCourseCommand")));
            services.AddTransient<ICommand, CreateSkillCommand>(c =>
                new CreateSkillCommand(c.GetRequiredService<ISkillService>(),
                ConfigurationManager.AppSettings.Get("CreateSkillCommand")));
            services.AddTransient<ICommand, CreateMaterialCommand>(c =>
                new CreateMaterialCommand(c.GetRequiredService<IMaterialService>(),
                ConfigurationManager.AppSettings.Get("CreateMaterialCommand")));
            services.AddTransient<ICommand, GetSkillsCommand>(c =>
                new GetSkillsCommand(c.GetRequiredService<ISkillService>(), defaultPageSize,
                ConfigurationManager.AppSettings.Get("GetSkillsCommand")));
            services.AddTransient<ICommand, GetMaterialsCommand>(c =>
                new GetMaterialsCommand(c.GetRequiredService<IMaterialService>(), defaultPageSize,
                ConfigurationManager.AppSettings.Get("GetMaterialsCommand")));
            services.AddTransient<ICommand, GetCoursesCommand>(c =>
                new GetCoursesCommand(c.GetRequiredService<ICourseService>(), defaultPageSize,
                ConfigurationManager.AppSettings.Get("GetCoursesCommand")));
            services.AddTransient<ICommand, GetAccountCourses>(c => 
                new GetAccountCourses(c.GetRequiredService<ICourseService>(), defaultPageSize,
                ConfigurationManager.AppSettings.Get("GetAccountCourses")));
            services.AddTransient<ICommand, AddSkillToCourseCommand>(c =>
                new AddSkillToCourseCommand(c.GetRequiredService<ICourseService>(),
                ConfigurationManager.AppSettings.Get("AddSkillToCourseCommand")));
            services.AddTransient<ICommand, AddMaterialToCourseCommand>(c =>
                new AddMaterialToCourseCommand(c.GetRequiredService<ICourseService>(),
                ConfigurationManager.AppSettings.Get("AddMaterialToCourseCommand")));
            services.AddTransient<ICommand, AddCourseToAccountCommand>(c =>
                new AddCourseToAccountCommand(c.GetRequiredService<IAccountService>(),
                ConfigurationManager.AppSettings.Get("AddCourseToAccountCommand")));
            services.AddTransient<ICommand, ShowAccountInfoCommand>(c =>
                new ShowAccountInfoCommand(c.GetRequiredService<IAccountService>(),
                ConfigurationManager.AppSettings.Get("ShowAccountInfoCommand")));
            services.AddTransient<ICommand, ChangeCourseStateCommand>(c =>
                new ChangeCourseStateCommand(c.GetRequiredService<ICourseService>(),
                ConfigurationManager.AppSettings.Get("ChangeCourseStateCommand")));
            services.AddTransient<ICommand, ShowCourseInfoCommand>(c =>
                new ShowCourseInfoCommand(c.GetRequiredService<ICourseService>(),
                ConfigurationManager.AppSettings.Get("ShowCourseInfoCommand")));
            services.AddTransient<ICommand, ShowMaterialInfoCommand>(c =>
                new ShowMaterialInfoCommand(c.GetRequiredService<IMaterialService>(),
                ConfigurationManager.AppSettings.Get("ShowMaterialInfoCommand")));
            services.AddTransient<ICommand, PassMaterialCommand>(c =>
                new PassMaterialCommand(c.GetRequiredService<IAccountService>(),
                ConfigurationManager.AppSettings.Get("PassMaterialCommand")));
            services.AddTransient<ICommand, PassCourseCommand>(c =>
                new PassCourseCommand(c.GetRequiredService<IAccountService>(),
                ConfigurationManager.AppSettings.Get("PassCourseCommand")));
            services.AddTransient<ICommand, PublishCourseCommand>(c =>
                new PublishCourseCommand(c.GetRequiredService<ICourseService>(),
                ConfigurationManager.AppSettings.Get("PublishCourseCommand")));

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
