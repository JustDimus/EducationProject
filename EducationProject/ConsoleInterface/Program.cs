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
using ADODataContext.DataContext;
using EducationProject.Core.DAL.EF;
using Infrastructure.DAL.EF.Mappings;
using EducationProject.EFCore;
using EducationProject.Core.PL;
using Infrastructure.BLL.EF;

namespace ConsoleInterface
{
    class Program
    {
        static IServiceCollection _services = new ServiceCollection();

        static void ConfigureServices(IServiceCollection services)
        {
            string xmlFileName = ConfigurationManager.AppSettings.Get("XMLFile");
            string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

            services.AddSingleton<EducationProjectDbContext>(x => new EducationProjectDbContext(connectionString));

            services.AddSingleton<XMLContext>(x => new XMLContext(xmlFileName));
            services.AddSingleton<ADOContext>(x => new ADOContext(connectionString));
            services.AddSingleton<UnitOfWork>();

            services.AddSingleton<IMapping<AccountBO>, AccountMapping>();
            services.AddSingleton<IMapping<CourseBO>, CourseMapping>();
            services.AddSingleton<IMapping<SkillBO>, SkillMapping>();
            services.AddSingleton<IMapping<BaseMaterial>, MaterialMapping>();

            services.AddSingleton<IMapping<AccountDBO>, AccountMappingEF>();
            services.AddSingleton<IMapping<SkillDBO>, SkillMappingEF>();
            services.AddSingleton<IMapping<BaseMaterialDBO>, MaterialMappingEF>();
            services.AddSingleton<IMapping<CourseDBO>, CourseMappingEF>();

            services.AddSingleton<IConverter<AccountDBO, AccountPL>, AccountConverter>();
            services.AddSingleton<IConverter<CourseDBO, CourseBO>, CourseConverter>();
            services.AddSingleton<IConverter<BaseMaterialDBO, BaseMaterial>, MaterialConverter>();
            services.AddSingleton<IConverter<SkillDBO, SkillBO>, SkillConverter>();

            services.AddSingleton<ICommand, AuthorizeAccountCommand>();
            services.AddSingleton<ICommand, CreateAccountCommand>();
            services.AddSingleton<ICommand, ErrorCommand>();
            services.AddSingleton<ICommand, CreateCourseCommand>();
            services.AddSingleton<ICommand, AuthenticateAccountCommand>();
            services.AddSingleton<ICommand, AddExistingCourseToAccountCommand>();
            services.AddSingleton<ICommand, AddExistingMaterialToCourseCommand>();
            services.AddSingleton<ICommand, AddExistingSkillToCourseCommand>();
            services.AddSingleton<ICommand, CreateMaterialCommand>();
            services.AddSingleton<ICommand, CreateSkillCommand>();
            services.AddSingleton<ICommand, IsAccountExistCommand>();
            services.AddSingleton<ICommand, IsCourseExistCommand>();
            services.AddSingleton<ICommand, IsMaterialExistCommand>();
            services.AddSingleton<ICommand, IsSkillExistCommand>();
            services.AddSingleton<ICommand, MoveCourseToPassedInAccountCommand>();
            services.AddSingleton<ICommand, ShowExistingAccountsCommand>();
            services.AddSingleton<ICommand, ShowExistingCoursesCommand>();
            services.AddSingleton<ICommand, ShowExistingSkillsCommand>();
            services.AddSingleton<ICommand, ShowExistingMaterialsCommand>();
            services.AddSingleton<ICommand, DeauthorizeAccountCommand>();
            services.AddSingleton<ICommand, ChangeCourseVisibilityCommand>();

            services.AddSingleton<AuthorizationService>();
            services.AddSingleton<AccountConverterService>();

            services.AddSingleton<IChain, CreateAccountChain>();
            services.AddSingleton<IChain, CreateCourseChain>();
            services.AddSingleton<IChain, AuthorizeAccountChain>();
            services.AddSingleton<IChain, ErrorChain>();
            services.AddSingleton<IChain, AddExistingCourseToAccountChain>();
            services.AddSingleton<IChain, AddExistingMaterialToCourseChain>();
            services.AddSingleton<IChain, AddExistingSkillToCourseChain>();
            services.AddSingleton<IChain, CreateMaterialChain>();
            services.AddSingleton<IChain, CreateSkillChain>();
            services.AddSingleton<IChain, MoveCourseToPassedInAccountChain>();
            services.AddSingleton<IChain, ShowExistingAccountsChain>();
            services.AddSingleton<IChain, ShowExistingCoursesChain>();
            services.AddSingleton<IChain, ShowExistingMaterialsChain>();
            services.AddSingleton<IChain, ShowExistingSkillsChain>();
            services.AddSingleton<IChain, DeauthorizeAccountChain>();
            services.AddSingleton<IChain, ChangeCourseVisibilityChain>();

            services.AddTransient<ICommandHandler, CommandHandler>();
            services.AddTransient<IConsoleHandler, ConsoleHandler>();
            services.AddTransient<IChainHandler, ChainHandler>();

            services.AddSingleton<AccountSectionHandler>();
            services.AddSingleton<CourseSectionHandler>();
            services.AddSingleton<SkillSectionHandler>();
            services.AddSingleton<MaterialSectionHandler>();
        }

        static void Main(string[] args)
        {
            ConfigureServices(_services);

            _services.BuildServiceProvider().GetService<IConsoleHandler>().Run();
        }
    }
}
