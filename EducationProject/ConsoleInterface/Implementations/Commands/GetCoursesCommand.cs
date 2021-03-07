using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleInterface.Validators;
using System.Threading.Tasks;

namespace ConsoleInterface.Implementations.Commands
{
    public class GetCoursesCommand : BaseCommand
    {
        private ICourseService courseService;

        private int defaultpageSize;

        private PageInfoValidator pageInfoValidator;

        public GetCoursesCommand(
            ICourseService courseService, 
            PageInfoValidator pageInfoValidator,
            int defaultPageSize, 
            string commandName)
            : base(commandName)
        {
            this.courseService = courseService;

            this.defaultpageSize = defaultPageSize;

            this.pageInfoValidator = pageInfoValidator;
        }

        public async override Task Run(int accountId)
        {
            Console.WriteLine("Getting courses");

            Console.Write("Enter the page: ");

            if (!int.TryParse(Console.ReadLine(), out int pageNumber))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
            }

            var pageInfo = new PageInfoDTO()
            {
                PageNumber = pageNumber,
                PageSize = this.defaultpageSize
            };

            if (!this.ValidateEntity(pageInfo))
            {
                return;
            }

            var coursesData = await this.courseService.GetAsync(pageInfo);

            if (!coursesData.IsSuccessful)
            {
                Console.WriteLine("Error");
                Console.WriteLine(coursesData.MessageCode);
                Console.WriteLine();
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendJoin(
                "\n",
                coursesData.Result.Select(c => $"{c.Id}: {c.Title}.\n\tDescription: {c.Description}"));

            Console.WriteLine(builder);
            Console.WriteLine();
        }

        private bool ValidateEntity(PageInfoDTO pageInfo)
        {
            var validationresult = this.pageInfoValidator.Validate(pageInfo);

            if (!validationresult.IsValid)
            {
                Console.WriteLine(string.Join("\n", validationresult.Errors));
                Console.WriteLine();
                return false;
            }

            return true;
        }
    }
}
