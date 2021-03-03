using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class GetMaterialsCommand : BaseCommand
    {
        private IMaterialService materialService;

        private int pageSize;

        public GetMaterialsCommand(IMaterialService materialService, 
            int defaultPageSize, string commandName)
            : base(commandName)
        {
            this.materialService = materialService;

            this.pageSize = defaultPageSize;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Getting materials");

            Console.Write("Enter the page: ");

            if (Int32.TryParse(Console.ReadLine(), out int pageNumber) == false)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
            }

            var materialsData = materialService.Get(new PageInfoDTO()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            if(materialsData == null)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendJoin("\n",
                materialsData.Select(m => m switch
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
    }
}
