﻿using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInterface.Implementations.Commands
{
    public class ShowMaterialInfoCommand : BaseCommand
    {
        private IMaterialService materialService;

        public ShowMaterialInfoCommand(
            IMaterialService materialService,
            string commandName)
            : base(commandName)
        {
            this.materialService = materialService;
        }

        public async override Task Run(int accountId)
        {
            Console.WriteLine("Showing material data");

            Console.Write("Material ID: ");

            if (!int.TryParse(Console.ReadLine(), out int materialId))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var actionResult = await this.materialService.GetMaterialInfoAsync(materialId);

            if (!actionResult.IsSuccessful)
            {
                Console.WriteLine("Error");
                Console.WriteLine(actionResult.MessageCode);
                Console.WriteLine();
                return;
            }

            StringBuilder builder = new StringBuilder();

            switch (actionResult.Result)
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
            Console.WriteLine();
        }
    }
}
