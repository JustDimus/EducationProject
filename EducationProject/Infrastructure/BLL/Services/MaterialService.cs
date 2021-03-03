using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using EducationProject.Core.DAL;
using EducationProject.DAL.Interfaces;
using Infrastructure.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

using MaterialType = EducationProject.Core.DAL.Enums.MaterialType;

namespace Infrastructure.BLL.Services
{
    public class MaterialService : BaseService<BaseMaterial, MaterialDTO>, IMaterialService
    {
        public MaterialService(IRepository<BaseMaterial> baseEntityRepository,
            AuthorizationService authorisztionService)
            : base(baseEntityRepository, authorisztionService)
        {
            
        }

        public MaterialDTO GetMaterialInfo(int id)
        {
            return entity.Get<MaterialDTO>(m => m.Id == id, FromBOMapping);
        }

        protected override Expression<Func<BaseMaterial, MaterialDTO>> FromBOMapping
        {
            get => bm => bm.Type == MaterialType.ArticleMaterial ? (MaterialDTO)new ArticleMaterialDTO()
            {
                Id = bm.Id,
                Description = bm.Description,
                Title = bm.Title,
                Type = bm.Type,
                URI = ((ArticleMaterial)bm).URI,
                PublicationDate = ((ArticleMaterial)bm).PublicationDate
            } :
            bm.Type == MaterialType.BookMaterial ? (MaterialDTO)new BookMaterialDTO()
            {
                Id = bm.Id,
                Description = bm.Description,
                Title = bm.Title,
                Type = bm.Type,
                Author = ((BookMaterial)bm).Author,
                Pages = ((BookMaterial)bm).Pages
            } :
            bm.Type == MaterialType.VideoMaterial ? (MaterialDTO)new VideoMaterialDTO()
            {
                Id = bm.Id,
                Description = bm.Description,
                Title = bm.Title,
                Type = bm.Type,
                URI = ((VideoMaterial)bm).URI,
                Duration = ((VideoMaterial)bm).Duration,
                Quality = ((VideoMaterial)bm).Quality
            } :
            null;
        }

        protected override BaseMaterial Map(MaterialDTO entity)
        {
            BaseMaterial material = null;

            switch(entity)
            {
                case ArticleMaterialDTO article:
                    material = new ArticleMaterial()
                    {
                        Id = article.Id,
                        Description = article.Description,
                        Title = article.Title,
                        Type = article.Type,
                        URI = article.URI,
                        PublicationDate = article.PublicationDate
                    };
                    break;
                case BookMaterialDTO book:
                    material = new BookMaterial()
                    {
                        Id = book.Id,
                        Description = book.Description,
                        Title = book.Title,
                        Type = book.Type,
                        Author = book.Author,
                        Pages = book.Pages
                    };
                    break;
                case VideoMaterialDTO video:
                    material = new VideoMaterial()
                    {
                        Id = video.Id,
                        Description = video.Description,
                        Title = video.Title,
                        Type = video.Type,
                        URI = video.URI,
                        Duration = video.Duration,
                        Quality = video.Quality
                    };
                    break;
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

        protected override Expression<Func<BaseMaterial, bool>> IsExistExpression(MaterialDTO entity)
        {
            return m => m.Id == entity.Id;
        }
    }
}
