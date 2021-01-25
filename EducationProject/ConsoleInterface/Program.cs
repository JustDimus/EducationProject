using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using XMLDataContext.DataContext;
using XMLDataContext.DataSets;
using EducationProject.Core.BLL;
using EducationProject.BLL.Interfaces;
using EducationProject.DAL.Mappings;
using Infrastructure.DAL.Mappings;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.BLL.Commands;
using Infrastructure.BLL;
using ConsoleInterface.Interfaces;
using ConsoleInterface.Realizations;
using Infrastructure.BLL.Chains;

namespace ConsoleInterface
{
    class Program
    {
        static IServiceCollection _services = new ServiceCollection();

        static void ConfigureServices(IServiceCollection Services)
        {
            Services.AddSingleton<XMLContext>();
            Services.AddSingleton<UnitOfWork>();

            Services.AddSingleton<IMapping<Account>, AccountMapping>();
            Services.AddSingleton<IMapping<Course>, CourseMapping>();
            Services.AddSingleton<IMapping<Skill>, SkillMapping>();
            Services.AddSingleton<IMapping<Material>, MaterialMapping>();

            Services.AddSingleton<ICommand, AuthorizeAccountCommand>();
            Services.AddSingleton<ICommand, CreateAccountCommand>();
            Services.AddSingleton<ICommand, ErrorCommand>();
            Services.AddSingleton<ICommand, CreateCourseCommand>();
            Services.AddSingleton<ICommand, AuthenticateAccountCommand>();

            Services.AddSingleton<AuthorizationService>();

            Services.AddTransient<IChain, CreateAccountChain>();
            Services.AddTransient<IChain, CreateCourseChain>();
            Services.AddTransient<IChain, AuthorizeAccountChain>();
            Services.AddTransient<IChain, ErrorChain>();

            Services.AddTransient<ICommandHandler, CommandHandler>();
            Services.AddTransient<IConsoleHandler, ConsoleHandler>();
            Services.AddTransient<IChainHandler, ChainHandler>();

        }

        static void Main(string[] args)
        {
            ConfigureServices(_services);

            _services.BuildServiceProvider().GetService<IConsoleHandler>().Run();
        }
    }
}
