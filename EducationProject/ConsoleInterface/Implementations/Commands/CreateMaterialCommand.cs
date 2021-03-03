using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class CreateMaterialCommand : BaseCommand
    {
        private IMaterialService materialService;

        public CreateMaterialCommand(IMaterialService materialService,
            string commandName)
            : base(commandName)
        {
            this.materialService = materialService;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Creating material");

            Console.WriteLine("Enter the type of material: 1 - Artcle, 2 - Book, 3 - Video");

            Int32.TryParse(Console.ReadLine(), out int type);

            MaterialDTO material = null;

            switch (type)
            {
                case 1:
                    material = GetArticleMaterial();
                    break;
                case 2:
                    material = GetBookMaterial();
                    break;
                case 3:
                    material = GetVideoMaterial();
                    break;
                default:
                    Console.WriteLine("Error. Enter the number!");
                    Console.WriteLine();
                    return;
            }

            if(material == null)
            {
                Console.WriteLine("Error. Invalid data!");
                Console.WriteLine();
                return;
            }

            var actionResult = this.materialService.Create(new ChangeEntityDTO<MaterialDTO>()
            {
                Token = token,
                Entity = material
            });

            if (actionResult == false)
            {
                Console.WriteLine("Error");
            }
            else
            {
                Console.WriteLine("Successful");
            }

            Console.WriteLine();
        }

        private MaterialDTO GetArticleMaterial()
        {
            Console.Write("Title: ");

            var title = Console.ReadLine();

            Console.Write("Description: ");

            var description = Console.ReadLine();

            Console.Write("URI: ");

            var URI = Console.ReadLine();

            Console.Write("Publication date (dd.MM.yyyy)");

            if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy",
                null, System.Globalization.DateTimeStyles.None, out DateTime publicationDate) == false)
            {
                return null;
            }

            if(String.IsNullOrEmpty(title) == true || String.IsNullOrEmpty(URI) == true)
            {
                return null;
            }

            return new ArticleMaterialDTO()
            {
                Title = title,
                Description = description,
                Type = EducationProject.Core.DAL.EF.Enums.MaterialType.ArticleMaterial,
                URI = URI,
                PublicationDate = publicationDate
            };
        }

        private MaterialDTO GetBookMaterial()
        {
            Console.Write("Title: ");

            var title = Console.ReadLine();

            Console.Write("Description: ");

            var description = Console.ReadLine();

            Console.Write("Author: ");

            var Author = Console.ReadLine();

            Console.Write("Pages: ");

            if (Int32.TryParse(Console.ReadLine(), out int pages) == false)
            {
                return null;
            }

            if(String.IsNullOrEmpty(title) == true)
            {
                return null;
            }

            return new BookMaterialDTO()
            {
                Title = title,
                Description = description,
                Type = EducationProject.Core.DAL.EF.Enums.MaterialType.BookMaterial,
                Author = Author,
                Pages = pages
            };
        }

        private MaterialDTO GetVideoMaterial()
        {
            Console.Write("Title: ");

            var title = Console.ReadLine();

            Console.Write("Description: ");

            var description = Console.ReadLine();

            Console.Write("URI: ");

            var URI = Console.ReadLine();

            Console.Write("Duration: ");

            if (Int32.TryParse(Console.ReadLine(), out int duration) == false)
            {
                return null;
            }

            Console.Write("Quality: ");

            if (Int32.TryParse(Console.ReadLine(), out int quality) == false)
            {
                return null;
            }

            if(String.IsNullOrEmpty(title) == true || String.IsNullOrEmpty(URI) == true)
            {
                return null;
            }

            return new VideoMaterialDTO()
            {
                Title = title,
                Description = description,
                Type = EducationProject.Core.DAL.EF.Enums.MaterialType.VideoMaterial,
                URI = URI,
                Duration = duration,
                Quality = quality
            };
        }
    }
}
