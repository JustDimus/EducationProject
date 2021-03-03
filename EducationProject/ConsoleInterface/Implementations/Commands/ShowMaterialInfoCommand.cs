using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class ShowMaterialInfoCommand : BaseCommand
    {
        private IMaterialService materials;

        public ShowMaterialInfoCommand(IMaterialService materialService,
            string commandName)
            : base(commandName)
        {
            this.materials = materialService;
        }

        public override void Run(ref string token)
        {
            int materialId = 0;

            Console.WriteLine("Showing material data");

            Console.Write("Material ID: ");

            if(Int32.TryParse(Console.ReadLine(), out materialId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var material = materials.GetMaterialInfo(materialId);

            StringBuilder builder = new StringBuilder();

            switch (material)
            {
                case ArticleMaterialDTO article:
                    builder.Append($"{article.Id}: {article.Title}.\n");
                    builder.Append($"\tURI: {article.URI}.\n");
                    break;
                case BookMaterialDTO book:
                    builder.Append($"{book.Id}: {book.Title}.\n");
                    builder.Append($"\tAuthor: {book.Author}. Pages: {book.Pages}\n");
                    break;
                case VideoMaterialDTO video:
                    builder.Append($"{video.Id}: {video.Title}.\n");
                    builder.Append($"\tURI: {video.URI}. Duration: {video.Duration}s.\n");
                    break;
            }

            Console.WriteLine(builder);
        }
    }
}
