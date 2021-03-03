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
        private IMaterialService materials;

        public CreateMaterialCommand(IMaterialService materialService,
            string commandName)
            : base(commandName)
        {
            materials = materialService;
        }

        public override void Run(ref string token)
        {
            int type = 0;

            MaterialDTO material = null;

            Console.WriteLine("Creating material");

            Console.WriteLine("Enter the type of material: 1 - Artcle, 2 - Book, 3 - Video");

            Int32.TryParse(Console.ReadLine(), out type);

            switch(type)
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

            if(material is null)
            {
                Console.WriteLine("Error. Invalid data!");
                Console.WriteLine();
                return;
            }

            if(materials.Create(new ChangeEntityDTO<MaterialDTO>()
            {
                Token = token,
                Entity = material
            }) == true)
            {
                Console.WriteLine("Successful");
            }
            else
            {
                Console.WriteLine("Error");
            }

            Console.WriteLine();
        }

        private MaterialDTO GetArticleMaterial()
        {
            string title = null;

            string description = null;

            string URI = null;

            DateTime publicationDate = default;

            Console.Write("Title: ");

            title = Console.ReadLine();

            Console.Write("Description: ");

            description = Console.ReadLine();

            Console.Write("URI: ");

            URI = Console.ReadLine();

            Console.Write("Publication date (dd.MM.yyyy)");

            if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy",
                null, System.Globalization.DateTimeStyles.None, out publicationDate) == false)
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
            string title = null;

            string description = null;

            string Author = null;

            int pages = 0;

            Console.Write("Title: ");

            title = Console.ReadLine();

            Console.Write("Description: ");

            description = Console.ReadLine();

            Console.Write("Author: ");

            Author = Console.ReadLine();

            Console.Write("Pages: ");

            if (Int32.TryParse(Console.ReadLine(), out pages) == false)
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
            string title = null;

            string description = null;

            string URI = null;

            int duration = 0;

            int quality = 0;

            Console.Write("Title: ");

            title = Console.ReadLine();

            Console.Write("Description: ");

            description = Console.ReadLine();

            Console.Write("URI: ");

            URI = Console.ReadLine();

            Console.Write("Duration: ");

            if (Int32.TryParse(Console.ReadLine(), out duration) == false)
            {
                return null;
            }

            Console.Write("Quality: ");

            if (Int32.TryParse(Console.ReadLine(), out quality) == false)
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
