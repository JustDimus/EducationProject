using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using EducationProject.Core.Models;
using EducationProject.DAL.Interfaces;
using Infrastructure.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

using MaterialType = EducationProject.Core.Models.Enums.MaterialType;
using System.Threading.Tasks;
using EducationProject.BLL;

namespace Infrastructure.BLL.Services
{
    public class MaterialService : IMaterialService
    {
        private IRepository<BaseMaterial> materialRepository;

        private IMapping<BaseMaterial, MaterialDTO> materialMapping;

        public MaterialService(IRepository<BaseMaterial> materialRepository,
            AuthorizationService authorizationService,
            IMapping<BaseMaterial, MaterialDTO> materialMapping)
        {
            this.materialRepository = materialRepository;

            this.materialMapping = materialMapping;

        }

        public async Task<IActionResult> CreateAsync(ChangeEntityDTO<MaterialDTO> createEntity)
        {
            await this.materialRepository.CreateAsync(materialMapping.Map(createEntity.Entity));

            return new ActionResult()
            {
                IsSuccessful = true
            };
        }

        public async Task<IActionResult<IEnumerable<MaterialDTO>>> GetAsync(PageInfoDTO pageInfo)
        {
            return new ActionResult<IEnumerable<MaterialDTO>>()
            {
                IsSuccessful = true,
                Result = await this.materialRepository.GetPageAsync<MaterialDTO>(m => true,
                materialMapping.ConvertExpression, pageInfo.PageNumber, pageInfo.PageSize)
            };
        }

        public async Task<IActionResult> DeleteAsync(ChangeEntityDTO<MaterialDTO> deleteEntity)
        {
            await materialRepository.DeleteAsync(materialMapping.Map(deleteEntity.Entity));

            return new ActionResult()
            {
                IsSuccessful = true
            };
        }

        public async Task<IActionResult<MaterialDTO>> GetMaterialInfo(int id)
        {
            return new ActionResult<MaterialDTO>()
            {
                IsSuccessful = true,
                Result = await this.materialRepository.GetAsync<MaterialDTO>(m => m.Id == id, 
                materialMapping.ConvertExpression)
            };
        }

        public async Task<IActionResult> IsExistAsync(MaterialDTO entity)
        {
            return new ActionResult<bool>()
            {
                IsSuccessful = true,
                Result = await this.materialRepository.AnyAsync(m => m.Id == entity.Id)
            };
        }

        public async Task<IActionResult> UpdateAsync(ChangeEntityDTO<MaterialDTO> changeEntity)
        {
            await this.materialRepository.UpdateAsync(materialMapping.Map(changeEntity.Entity));

            return new ActionResult()
            {
                IsSuccessful = true
            };
        }
    }
}
