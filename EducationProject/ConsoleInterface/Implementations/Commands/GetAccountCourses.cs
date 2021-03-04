using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleInterface.Validators;

namespace ConsoleInterface.Implementations.Commands
{
    public class GetAccountCourses : BaseCommand
    {        
        private ICourseService courseService;

        private int pageSize;

        private GetCoursesByCreatorValidator getCoursesByCreatorValidator;

        public GetAccountCourses(ICourseService courseService,
            GetCoursesByCreatorValidator getCoursesByCreatorValidator,
            int defaultPageSize, string commandName)
            : base(commandName)
        {
            this.courseService = courseService;

            this.pageSize = defaultPageSize;

            this.getCoursesByCreatorValidator = getCoursesByCreatorValidator;
        }

        public async override void Run(int accountId)
        {
            Console.WriteLine("Getting courses");

            Console.Write("Enter the page: ");

            if (!Int32.TryParse(Console.ReadLine(), out int pageNumber))
            {
                Console.WriteLine("Error");
                Console.WriteLine();
            }

            var getCourses = new GetCoursesByCreatorDTO()
            {
                AccountId = accountId,
                PageInfo = new PageInfoDTO()
                {
                    PageSize = pageSize,
                    PageNumber = pageNumber
                }
            };

            if(!this.ValidateEntity(getCourses))
            {
                return;
            }

            var coursesData = await this.courseService.GetCoursesByCreatorIdAsync(getCourses);

            if(!coursesData.IsSuccessful)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
                return;
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendJoin("\n",
                coursesData.Result.Select(c => $"{c.Id}: {c.Title}.\n\tDescription: {c.Description}"));

            Console.WriteLine(builder);
            Console.WriteLine();
        }

        private bool ValidateEntity(GetCoursesByCreatorDTO getCourses)
        {
            var validationresult = this.getCoursesByCreatorValidator.Validate(getCourses);

            if (!validationresult.IsValid)
            {
                Console.WriteLine(String.Join("\n", validationresult.Errors));
                Console.WriteLine();
                return false;
            }

            return true;
        }
    }
}
