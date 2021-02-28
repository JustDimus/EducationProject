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

        public MaterialDTO Get(int id)
        {
            return entity.Get<MaterialDTO>(m => m.Id == id, FromBOMapping);
        }

        protected override Expression<Func<BaseMaterialDBO, MaterialDTO>> FromBOMapping
        {
            get => bm => bm.Type == MaterialType.ArticleMaterial ? (MaterialDTO)new ArticleMaterialDTO()
            {
                Id = bm.Id,
                Description = bm.Description,
                Title = bm.Title,
                Type = bm.Type,
                URI = ((ArticleMaterialDBO)bm).URI,
                PublicationDate = ((ArticleMaterialDBO)bm).PublicationDate
            } :
            bm.Type == MaterialType.BookMaterial ? (MaterialDTO)new BookMaterialDTO()
            {
                Id = bm.Id,
                Description = bm.Description,
                Title = bm.Title,
                Type = bm.Type,
                Author = ((BookMaterialDBO)bm).Author,
                Pages = ((BookMaterialDBO)bm).Pages
            } :
            bm.Type == MaterialType.VideoMaterial ? (MaterialDTO)new VideoMaterialDTO()
            {
                Id = bm.Id,
                Description = bm.Description,
                Title = bm.Title,
                Type = bm.Type,
                URI = ((VideoMaterialDBO)bm).URI,
                Duration = ((VideoMaterialDBO)bm).Duration,
                Quality = ((VideoMaterialDBO)bm).Quality
            } :
            null;
        }

        protected override BaseMaterialDBO Map(MaterialDTO entity)
        {
            BaseMaterialDBO material = null;

            switch(entity)
            {
                case ArticleMaterialDTO article:
                    material = new ArticleMaterialDBO()
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
                    material = new BookMaterialDBO()
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
                    material = new VideoMaterialDBO()
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
    }
}
