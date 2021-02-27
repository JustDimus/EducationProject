using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using EducationProject.Core.DAL.EF;
using Infrastructure.DAL.EF.Mappings;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

using MaterialType = EducationProject.Core.DAL.EF.Enums.MaterialType;

namespace Infrastructure.BLL.Services
{
    public class MaterialService : BaseService<BaseMaterialDBO, MaterialDTO>, IMaterialService
    {
        public MaterialService(BaseRepository<BaseMaterialDBO> baseEntityRepository, 
            AuthorizationService authorisztionService) 
            : base(baseEntityRepository, authorisztionService)
        {

        }

        protected override Expression<Func<BaseMaterialDBO, MaterialDTO>> FromBOMapping => throw new NotFiniteNumberException();
        
        protected override Expression<Func<BaseMaterialDBO, MaterialDTO>> FullMap => FromBOMapping;

        protected override Func<MaterialDTO, Expression<Func<BaseMaterialDBO, bool>>> getObjectInfoCondition
        {
            get => GetExpression;
        }

        protected override BaseMaterialDBO Map(MaterialDTO entity)
        {
            BaseMaterialDBO material = new BaseMaterialDBO()
            {
                Id = entity.Id,
                Description = entity.Description,
                Title = entity.Title,
                Type = entity.Type
            };

            switch(entity)
            {
                case ArticleMaterialDTO article:
                    material.Article = new ArticleMaterialDBO()
                    {
                        URI = article.URI,
                        PublicationDate = article.PublicationDate
                    };
                    break;
                case BookMaterialDTO book:
                    material.Book = new BookMaterialDBO()
                    {
                        Author = book.Author,
                        Pages = book.Pages
                    };
                    break;
                case VideoMaterialDTO video:
                    material.Video = new VideoMaterialDBO()
                    {
                        URI = video.URI,
                        Duration = video.Duration,
                        Quality = video.Quality
                    };
                    break;
                default:
                    return null;
            }

            return material;
        }

        protected override bool ValidateEntity(MaterialDTO entity)
        {
            if(String.IsNullOrEmpty(entity.Title) == true)
            {
                return false;
            }

            switch(entity.Type)
            {
                case MaterialType.ArticleMaterial:
                    if(entity is ArticleMaterialDTO article)
                    {
                        if(String.IsNullOrEmpty(article.URI) == true)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case MaterialType.BookMaterial:
                    if(entity is BookMaterialDTO book)
                    {
                        if(book.Pages <= 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case MaterialType.VideoMaterial:
                    if(entity is VideoMaterialDTO video)
                    {
                        if(video.Duration < 0 || video.Quality < 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                    break;
                default:
                    return false;
            }

            return true;
        }

        private Expression<Func<BaseMaterialDBO, bool>> GetExpression(MaterialDTO material)
        {
            return m => m.Id == material.Id;
        }
    }
}
