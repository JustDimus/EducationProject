using EducationProject.BLL.DTO;
using EducationProject.BLL.Interfaces;
using EducationProject.Core.Models;
using EducationProject.Core.Models.Enums;
using System;
using System.Linq.Expressions;

namespace EducationProject.Infrastructure.BLL.Mappings
{
    public class MaterialMapping : IMapping<BaseMaterial, MaterialDTO>
    {
        public Expression<Func<BaseMaterial, MaterialDTO>> ConvertExpression
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

        public BaseMaterial Map(MaterialDTO externalEntity)
        {
            BaseMaterial material = null;

            switch (externalEntity)
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
    }
}
