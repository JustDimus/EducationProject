using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleInterface.Validators;
using System.Threading.Tasks;

namespace ConsoleInterface.Implementations.Commands
{
    public class CreateMaterialCommand : BaseCommand
    {
        private IMaterialService materialService;

        private ChangeEntityValidator<MaterialDTO> changeEntityValidator;

        public CreateMaterialCommand(
            IMaterialService materialService,
            ChangeEntityValidator<MaterialDTO> changeEntityValidator,
            string commandName)
            : base(commandName)
        {
            this.materialService = materialService;

            this.changeEntityValidator = changeEntityValidator;
        }

        public async override Task Run(int accountId)
        {
            Console.WriteLine("Creating material");

            Console.WriteLine("Enter the type of material: 1 - Artcle, 2 - Book, 3 - Video");

            if (!int.TryParse(Console.ReadLine(), out int type))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
            }

            var changeEntity = new ChangeEntityDTO<MaterialDTO>()
            {
                AccountId = accountId
            };

            switch (type)
            {
                case 1:
                    changeEntity.Entity = this.GetArticleMaterial();
                    break;
                case 2:
                    changeEntity.Entity = this.GetBookMaterial();
                    break;
                case 3:
                    changeEntity.Entity = this.GetVideoMaterial();
                    break;
            }

            if (!this.ValidateEntity(changeEntity))
            {
                return;
            }

            var actionResult = await this.materialService.CreateAsync(changeEntity);

            if (!actionResult.IsSuccessful)
            {
                Console.WriteLine("Error");
                Console.WriteLine(actionResult.MessageCode);
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

            Console.Write("Publication date. Enter date in next format (dd.mm.yyyy)");

            if (!DateTime.TryParseExact(
                Console.ReadLine(), 
                "dd.MM.yyyy",
                null, 
                System.Globalization.DateTimeStyles.None, 
                out DateTime publicationDate))
            {
                return null;
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(URI))
            {
                return null;
            }

            return new ArticleMaterialDTO()
            {
                Title = title,
                Description = description,
                Type = EducationProject.Core.Models.Enums.MaterialType.ArticleMaterial,
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

            if (!int.TryParse(Console.ReadLine(), out int pages))
            {
                return null;
            }

            if (string.IsNullOrEmpty(title))
            {
                return null;
            }

            return new BookMaterialDTO()
            {
                Title = title,
                Description = description,
                Type = EducationProject.Core.Models.Enums.MaterialType.BookMaterial,
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

            if (!int.TryParse(Console.ReadLine(), out int duration))
            {
                return null;
            }

            Console.Write("Quality: ");

            if (!int.TryParse(Console.ReadLine(), out int quality))
            {
                return null;
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(URI))
            {
                return null;
            }

            return new VideoMaterialDTO()
            {
                Title = title,
                Description = description,
                Type = EducationProject.Core.Models.Enums.MaterialType.VideoMaterial,
                URI = URI,
                Duration = duration,
                Quality = quality
            };
        }

        private bool ValidateEntity(ChangeEntityDTO<MaterialDTO> changeEntity)
        {
            var validationresult = this.changeEntityValidator.Validate(changeEntity);

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
