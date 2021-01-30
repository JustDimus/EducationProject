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
using System.Configuration;

namespace ConsoleInterface
{
    class Program
    {
        static IServiceCollection _services = new ServiceCollection();

        static void ConfigureServices(IServiceCollection Services)
        {
            string xmlFileName = ConfigurationManager.AppSettings.Get("XMLFile");

            Services.AddSingleton<XMLContext>(x => new XMLContext(xmlFileName));
            Services.AddSingleton<UnitOfWork>();

            Services.AddSingleton<IMapping<AccountBO>, AccountMapping>();
            Services.AddSingleton<IMapping<CourseBO>, CourseMapping>();
            Services.AddSingleton<IMapping<SkillBO>, SkillMapping>();
            Services.AddSingleton<IMapping<BaseMaterial>, MaterialMapping>();

            Services.AddSingleton<ICommand, AuthorizeAccountCommand>();
            Services.AddSingleton<ICommand, CreateAccountCommand>();
            Services.AddSingleton<ICommand, ErrorCommand>();
            Services.AddSingleton<ICommand, CreateCourseCommand>();
            Services.AddSingleton<ICommand, AuthenticateAccountCommand>();
            Services.AddSingleton<ICommand, AddExistingCourseToAccountCommand>();
            Services.AddSingleton<ICommand, AddExistingMaterialToCourseCommand>();
            Services.AddSingleton<ICommand, AddExistingSkillToCourseCommand>();
            Services.AddSingleton<ICommand, CreateMaterialCommand>();
            Services.AddSingleton<ICommand, CreateSkillCommand>();
            Services.AddSingleton<ICommand, IsAccountExistCommand>();
            Services.AddSingleton<ICommand, IsCourseExistCommand>();
            Services.AddSingleton<ICommand, IsMaterialExistCommand>();
            Services.AddSingleton<ICommand, IsSkillExistCommand>();
            Services.AddSingleton<ICommand, MoveCourseToPassedInAccountCommand>();
            Services.AddSingleton<ICommand, ShowExistingAccountsCommand>();
            Services.AddSingleton<ICommand, ShowExistingCoursesCommand>();
            Services.AddSingleton<ICommand, ShowExistingSkillsCommand>();
            Services.AddSingleton<ICommand, ShowExistingMaterialsCommand>();
            Services.AddSingleton<ICommand, DeauthorizeAccountCommand>();
            Services.AddSingleton<ICommand, ChangeCourseVisibilityCommand>();

            Services.AddSingleton<AuthorizationService>();
            Services.AddSingleton<AccountConverterService>();

            Services.AddSingleton<IChain, CreateAccountChain>();
            Services.AddSingleton<IChain, CreateCourseChain>();
            Services.AddSingleton<IChain, AuthorizeAccountChain>();
            Services.AddSingleton<IChain, ErrorChain>();
            Services.AddSingleton<IChain, AddExistingCourseToAccountChain>();
            Services.AddSingleton<IChain, AddExistingMaterialToCourseChain>();
            Services.AddSingleton<IChain, AddExistingSkillToCourseChain>();
            Services.AddSingleton<IChain, CreateMaterialChain>();
            Services.AddSingleton<IChain, CreateSkillChain>();
            Services.AddSingleton<IChain, MoveCourseToPassedInAccountChain>();
            Services.AddSingleton<IChain, ShowExistingAccountsChain>();
            Services.AddSingleton<IChain, ShowExistingCoursesChain>();
            Services.AddSingleton<IChain, ShowExistingMaterialsChain>();
            Services.AddSingleton<IChain, ShowExistingSkillsChain>();
            Services.AddSingleton<IChain, DeauthorizeAccountChain>();
            Services.AddSingleton<IChain, ChangeCourseVisibilityChain>();

            Services.AddTransient<ICommandHandler, CommandHandler>();
            Services.AddTransient<IConsoleHandler, ConsoleHandler>();
            Services.AddTransient<IChainHandler, ChainHandler>();

            Services.AddSingleton<AccountSectionHandler>();
            Services.AddSingleton<CourseSectionHandler>();
            Services.AddSingleton<SkillSectionHandler>();
            Services.AddSingleton<MaterialSectionHandler>();
        }

        static void Main(string[] args)
        {
            ConfigureServices(_services);

            _services.BuildServiceProvider().GetService<IConsoleHandler>().Run();
        }
    }
}
