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
    public class MateriaService : BaseService<BaseMaterialDBO, MaterialDTO>, IBusinessService<MaterialDTO>
    {
        public MateriaService(BaseRepository<BaseMaterialDBO> materials, 
            AuthorizationService authService)
            : base(materials, authService)
        {

        }

        private Expression<Func<BaseMaterialDBO, MaterialDTO>> mappingExpression
        {
            get => bm => new MaterialDTO()
            {
                Id = bm.Id,
                Description = bm.Description,
                Title = bm.Title,
                Type = bm.Type
            };
        }

        protected override Expression<Func<BaseMaterialDBO, MaterialDTO>> FromBOMapping => throw new NotImplementedException();

        protected override Expression<Func<BaseMaterialDBO, MaterialDTO>> FullMap => throw new NotImplementedException();

        protected override Func<MaterialDTO, Expression<Func<BaseMaterialDBO, bool>>> getObjectInfoCondition => throw new NotImplementedException();

        public bool Create(ChangeEntityDTO<MaterialDTO> createMaterial)
        {
            if(authService.AuthenticateAccount(createMaterial.Token) == 0)
            {
                return false;
            }

            entity.Create(Map(createMaterial.Entity));

            entity.Save();

            return true;
        }

        public bool Update(ChangeEntityDTO<MaterialDTO> updateMaterial)
        {
            if (authService.AuthenticateAccount(updateMaterial.Token) == 0)
            {
                return false;
            }

            entity.Update(Map(updateMaterial.Entity));

            entity.Save();

            return true;
        }

        public bool Delete(ChangeEntityDTO<MaterialDTO> deleteMaterial)
        {
            if (authService.AuthenticateAccount(deleteMaterial.Token) == 0)
            {
                return false;
            }

            entity.Delete(deleteMaterial.Entity.Id);

            entity.Save();

            return true;
        }

        public IEnumerable<MaterialDTO> Get(PageInfoDTO pageInfo)
        {
            if (ValidatePageInfo(pageInfo) == false)
            {
                return null;
            }

            return entity.GetPage<MaterialDTO>(bm => true, mappingExpression, pageInfo.PageNumber, pageInfo.PageSize);
        }

        public MaterialDTO GetInfo(MaterialDTO entity)
        {
            throw new NotImplementedException();
        }

        protected override BaseMaterialDBO Map(MaterialDTO changeMaterial)
        {
            BaseMaterialDBO material = new BaseMaterialDBO()
            {
                Id = changeMaterial.Id,
                Title = changeMaterial.Title,
                Description = changeMaterial.Description,
                Type = changeMaterial.Type
            };
            /*
            switch (changeMaterial)
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
            }*/

            return material;
        }

        protected override bool ValidateEntity(MaterialDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}
