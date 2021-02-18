using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.BLL.EF
{
    public class MaterialConverter : BaseConverter<BaseMaterialDBO, BaseMaterial>
    {

        public MaterialConverter(IMapping<BaseMaterialDBO> materialMapping)
            :base(materialMapping)
        {

        }

        public override BaseMaterial Get(BaseMaterialDBO entity)
        {
            switch(entity.Type)
            {
                case EducationProject.Core.DAL.EF.Enums.MaterialType.ArticleMaterial:
                    return new ArticleMaterial()
                    {
                        Id = entity.Id,
                        Description = entity.Description,
                        Title = entity.Title,
                        Type = entity.Type,
                        ArticleData = new ArticleData()
                        {
                            PublicationDate = entity.Article.PublicationDate,
                            URI = entity.Article.URI
                        }
                    };
                case EducationProject.Core.DAL.EF.Enums.MaterialType.BookMaterial:
                    return new BookMaterial()
                    {
                        Id = entity.Id,
                        Description = entity.Description,
                        Title = entity.Title,
                        Type = entity.Type,
                        BookData = new BookData()
                        {
                            Author = entity.Book.Author,
                            Pages = entity.Book.Pages
                        }
                    };
                case EducationProject.Core.DAL.EF.Enums.MaterialType.VideoMaterial:
                    return new VideoMaterial()
                    {
                        Id = entity.Id,
                        Description = entity.Description,
                        Title = entity.Title,
                        Type = entity.Type,
                        VideoData = new VideoData()
                        {
                            Duration = entity.Video.Duration,
                            Quality = entity.Video.Quality,
                            URI = entity.Video.URI
                        }
                    };
                default:
                    throw new ArgumentException();
            }
        }
    }
}
