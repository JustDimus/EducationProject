using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using EducationProject.EFCore;
using EducationProject.Core.Models;
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

            services.AddTransient<IRepository<Course>, BaseRepository<Course>>();
            services.AddTransient<IRepository<Account>, BaseRepository<Account>>();
            services.AddTransient<IRepository<Skill>, BaseRepository<Skill>>();
            services.AddTransient<IRepository<BaseMaterial>, BaseRepository<BaseMaterial>>();
            services.AddTransient<IRepository<CourseSkill>, BaseRepository<CourseSkill>>();
            services.AddTransient<IRepository<CourseMaterial>, BaseRepository<CourseMaterial>>();
            services.AddTransient<IRepository<AccountCourse>, BaseRepository<AccountCourse>>();
            services.AddTransient<IRepository<AccountMaterial>, BaseRepository<AccountMaterial>>();
            services.AddTransient<IRepository<AccountSkill>, BaseRepository<AccountSkill>>();

            services.AddSingleton<AuthorizationService>();

            services.AddTransient<ISkillService, SkillService>();
            services.AddTransient<ICourseService, CourseService>(c => 
                new CourseService(c.GetRequiredService<IRepository<Course>>(),
                c.GetRequiredService<AuthorizationService>(),
                c.GetRequiredService<IMaterialService>(),
                c.GetRequiredService<ISkillService>(),
                c.GetRequiredService<IRepository<CourseSkill>>(),
                c.GetRequiredService<IRepository<CourseMaterial>>(),
                defaultPageSize));
            services.AddTransient<IMaterialService, MaterialService>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddTransient<ICommandHandler, CommandHandler>(c =>
                new CommandHandler(c.GetRequiredService<IEnumerable<ICommand>>(),
                ConfigurationManager.AppSettings.Get("ShowAllCommands"),
                ConfigurationManager.AppSettings.Get("InvalidCommand")));

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

            services.AddSingleton<IConsoleHandler, ConsoleHandler>(c =>
                new ConsoleHandler(c.GetRequiredService<ICommandHandler>(),
                ConfigurationManager.AppSettings.Get("ExitCommand")));
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
