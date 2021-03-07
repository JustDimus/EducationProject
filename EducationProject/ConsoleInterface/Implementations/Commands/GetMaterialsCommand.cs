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
    public class GetMaterialsCommand : BaseCommand
    {
        private IMaterialService materialService;

        private int pageSize;

        private PageInfoValidator pageInfoValidator;

        public GetMaterialsCommand(IMaterialService materialService,
            PageInfoValidator pageInfoValidator,
            int defaultPageSize, string commandName)
            : base(commandName)
        {
            this.materialService = materialService;

            this.pageSize = defaultPageSize;

            this.pageInfoValidator = pageInfoValidator;
        }

        public async override Task Run(int accountId)
        {
            Console.WriteLine("Getting materials");

            Console.Write("Enter the page: ");

            if (!Int32.TryParse(Console.ReadLine(), out int pageNumber))
            {
                Console.WriteLine("Error");
                Console.WriteLine();
            }

            var pageInfo = new PageInfoDTO()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            if(!this.ValidateEntity(pageInfo))
            {
                return;
            }

            var materialsData = await materialService.GetAsync(pageInfo);

            if(!materialsData.IsSuccessful)
            {
                Console.WriteLine("Error");
                Console.WriteLine(materialsData.MessageCode);
                Console.WriteLine();
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendJoin("\n",
                materialsData.Result.Select(m => m switch
                {
                    ArticleMaterialDTO article => new StringBuilder()
                        .Append($"{article.Id}: {article.Title}. Type: Article.\n")
                        .Append($"\tURI: {article.URI}."),

                    BookMaterialDTO book => new StringBuilder()
                        .Append($"{book.Id}: {book.Title}. Type: Book.\n")
                        .Append($"\tAuthor: {book.Author}. Pages: {book.Pages}"),

                    VideoMaterialDTO video => new StringBuilder()
                        .Append($"{video.Id}: {video.Title}. Type: Video.\n")
                        .Append($"\tURI: {video.URI}. Duration: {video.Duration}s."),

                    _ => null
                }));

            Console.WriteLine(builder);
        }

        private bool ValidateEntity(PageInfoDTO pageInfo)
        {
            var validationresult = this.pageInfoValidator.Validate(pageInfo);

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
