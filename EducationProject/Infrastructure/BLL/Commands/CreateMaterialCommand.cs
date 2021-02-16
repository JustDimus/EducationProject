using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
using EducationProject.Core.DAL.EF.Enums;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class CreateMaterialCommand : ICommand
    {
        public string Name => "CreateMaterial";

        IMapping<BaseMaterialDBO> materials;

        public CreateMaterialCommand(IMapping<BaseMaterialDBO> materialMapping)
        {
            materials = materialMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            AccountAuthenticationData account = Params[0] as AccountAuthenticationData;

            string title = Params[1] as string;

            string description = Params[2] as string;

            MaterialType? type = Params[3] as MaterialType?;

            if (String.IsNullOrEmpty(title) || type.HasValue == false || account is null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid material data: CreateMaterialCommand"
                };
            }

            if (type == MaterialType.VideoMaterial && Params.Length < 7)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data count for material type Video: CreateMaterialCommand"
                };
            }

            BaseMaterialDBO material = new BaseMaterialDBO()
            {
                Title = title,
                Description = description,
                Type = type.Value
            };

            switch (type)
            {
                case MaterialType.VideoMaterial:
                    material.Video = new VideoMaterialDBO()
                    {
                        URI = Params[4] as string,
                        Duration = (int)Params[5],
                        Quality = (int)Params[6]
                    };

                    break;
                case MaterialType.ArticleMaterial:
                    material.Article = new ArticleMaterialDBO()
                    {
                        URI = Params[4] as string,
                        PublicationDate = (DateTime)Params[5]
                    };

                    break;
                case MaterialType.BookMaterial:
                    material.Book = new BookMaterialDBO()
                    {
                        Author = Params[4] as string,
                        Pages = (int)Params[5]
                    };

                    break;
                default:
                    return new OperationResult()
                    {
                        Status = ResultType.Failed,
                        Result = $"Invalid material type: CreateMaterialCommand"
                    };
            }

            materials.Create(material);

            materials.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Created material by account {account.AccountId} with name {material.Title}. Id: {material.Id}"
            };
        }
    }
}
