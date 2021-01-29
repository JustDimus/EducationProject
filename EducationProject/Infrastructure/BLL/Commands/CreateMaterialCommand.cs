using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
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

        IMapping<BaseMaterial> _materials;

        public CreateMaterialCommand(IMapping<BaseMaterial> materials)
        {
            _materials = materials;
        }

        public IOperationResult Handle(object[] Params)
        {
            AccountAuthenticationData account = Params[0] as AccountAuthenticationData;

            string title = Params[1] as string;

            string description = Params[2] as string;

            string type = Params[3] as string;

            if (String.IsNullOrEmpty(title) || String.IsNullOrEmpty(type))
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid material data: CreateMaterialCommand"
                };
            }

            if (type == "Video" && Params.Length < 7)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data count for material type Video: CreateMaterialCommand"
                };
            }

            BaseMaterial material = null;

            switch (type)
            {
                case "Video":
                    material = new VideoMaterial()
                    {
                        Title = title,
                        Description = description,
                        Type = type,
                        VideoData = new VideoData()
                        {
                            URI = Params[4] as string,
                            Duration = (int)Params[5],
                            Quality = (int)Params[6]
                        }
                    };
                    break;
                case "Article":
                    material = new ArticleMaterial()
                    {
                        Title = title,
                        Description = description,
                        Type = type,
                        ArticleData = new ArticleData()
                        {
                            URI = Params[4] as string,
                            PublicationDate = (DateTime)Params[5]
                        }
                    };
                    break;
                case "Book":
                    material = new BookMaterial()
                    {
                        Title = title,
                        Description = description,
                        Type = type,
                        BookData = new BookData()
                        { 
                            Author = Params[4] as string,
                            Pages = (int)Params[5]
                        }
                    };
                    break;
                default:
                    return new OperationResult()
                    {
                        Status = ResultType.Failed,
                        Result = $"Invalid material type: CreateMaterialCommand"
                    };
            }

            _materials.Create(material);

            _materials.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Created material by account {account.AccountData.Id} with name {material.Title}. Id: {material.Id}"
            };
        }
    }
}
