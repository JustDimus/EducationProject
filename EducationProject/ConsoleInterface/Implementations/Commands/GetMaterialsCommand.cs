using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class GetMaterialsCommand : BaseCommand
    {
        private IMaterialService materials;

        private int pageSize;

        public GetMaterialsCommand(IMaterialService materialService, 
            int defaultPageSize, string commandName)
            : base(commandName)
        {
            this.materials = materialService;

            this.pageSize = defaultPageSize;
        }

        public override void Run(ref string token)
        {
            int pageNumber = 0;

            Console.WriteLine("Getting materials");

            Console.Write("Enter the page: ");

            Int32.TryParse(Console.ReadLine(), out pageNumber);

            var materialsData = materials.Get(new PageInfoDTO()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            StringBuilder builder = new StringBuilder();

            foreach(var material in materialsData)
            {
                switch(material)
                {
                    case ArticleMaterialDTO article:
                        builder.Append($"{article.Id}: {article.Title}. Type: Article.\n");
                        builder.Append($"\tURI: {article.URI}.\n");
                        break;
                    case BookMaterialDTO book:
                        builder.Append($"{book.Id}: {book.Title}. Type: Book.\n");
                        builder.Append($"\tAuthor: {book.Author}. Pages: {book.Pages}\n");
                        break;
                    case VideoMaterialDTO video:
                        builder.Append($"{video.Id}: {video.Title}. Type: Video.\n");
                        builder.Append($"\tURI: {video.URI}. Duration: {video.Duration}s.\n");
                        break;
                }
            }

            Console.WriteLine(builder);
        }
    }
}
