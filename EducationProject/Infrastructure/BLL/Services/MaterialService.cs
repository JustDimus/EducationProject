using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using EducationProject.Core.Models;
using EducationProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EducationProject.BLL;
using System.Linq;
using EducationProject.BLL.ActionResultMessages;

namespace EducationProject.Infrastructure.BLL.Services
{
    public class MaterialService : BaseService, IMaterialService
    {
        private IRepository<BaseMaterial> materialRepository;

        private IRepository<CourseMaterial> courseMaterialRepository;

        private IRepository<AccountMaterial> accountMaterialRepository;

        private IMapping<BaseMaterial, MaterialDTO> materialMapping;

        private MaterialServiceActionResultMessages materialResultMessages;

        public MaterialService(
            IRepository<BaseMaterial> materialRepository,
            AuthorizationService authorizationService,
            IMapping<BaseMaterial, MaterialDTO> materialMapping,
            IRepository<CourseMaterial> courseMaterialRepository,
            IRepository<AccountMaterial> accountMaterialRepository,
            MaterialServiceActionResultMessages materialActionResultMessages)
        {
            this.materialRepository = materialRepository;

            this.materialMapping = materialMapping;

            this.courseMaterialRepository = courseMaterialRepository;

            this.accountMaterialRepository = accountMaterialRepository;

            this.materialResultMessages = materialActionResultMessages;
        }

        public async Task<IActionResult> CreateAsync(ChangeEntityDTO<MaterialDTO> createEntity)
        {
            var newMaterial = this.materialMapping.Map(createEntity.Entity);

            await this.materialRepository.CreateAsync(newMaterial);

            return this.GetDefaultActionResult(true);
        }

        public async Task<IActionResult<IEnumerable<MaterialDTO>>> GetAsync(PageInfoDTO pageInfo)
        {
            return new ActionResult<IEnumerable<MaterialDTO>>()
            {
                IsSuccessful = true,
                Result = await this.materialRepository.GetPageAsync<MaterialDTO>(
                    m => true,
                    this.materialMapping.ConvertExpression, 
                    pageInfo.PageNumber, 
                    pageInfo.PageSize)
            };
        }

        public async Task<IActionResult> DeleteAsync(ChangeEntityDTO<MaterialDTO> deleteEntity)
        {
            await this.materialRepository.DeleteAsync(this.materialMapping.Map(deleteEntity.Entity));

            return this.GetDefaultActionResult(true);
        }

        public async Task<IActionResult<MaterialDTO>> GetMaterialInfoAsync(int id)
        {
            return new ActionResult<MaterialDTO>()
            {
                IsSuccessful = true,
                Result = await this.materialRepository.GetAsync<MaterialDTO>(
                    m => m.Id == id,
                    this.materialMapping.ConvertExpression)
            };
        }

        public async Task<IActionResult<bool>> IsExistAsync(MaterialDTO entity)
        {
            return new ActionResult<bool>()
            {
                IsSuccessful = true,
                Result = await this.materialRepository.AnyAsync(m => m.Id == entity.Id)
            };
        }

        public async Task<IActionResult> UpdateAsync(ChangeEntityDTO<MaterialDTO> changeEntity)
        {
            var isMaterialExist = await this.materialRepository.AnyAsync(m =>
                m.Id == changeEntity.Entity.Id);

            if (!isMaterialExist)
            {
                return this.GetDefaultActionResult(false, this.materialResultMessages.MaterialNotExist);
            }

            await this.materialRepository.UpdateAsync(this.materialMapping.Map(changeEntity.Entity));

            return this.GetDefaultActionResult(true);
        }

        public async Task<IActionResult<IEnumerable<MaterialDTO>>> GetAllCourseMaterialsAsync(int courseId)
        {
            var i = await this.courseMaterialRepository.GetPageAsync<int>(
                cm => cm.CourseId == courseId,
                cm => cm.MaterialId, 
                0, 
                await this.courseMaterialRepository.CountAsync(cm => cm.CourseId == courseId));

            return new ActionResult<IEnumerable<MaterialDTO>>()
            {
                IsSuccessful = true,
                Result = await this.materialRepository.GetPageAsync<MaterialDTO>(
                    m => i.Contains(m.Id), 
                    this.materialMapping.ConvertExpression, 
                    0,
                    await this.materialRepository.CountAsync(m => i.Contains(m.Id)))
            };
        }

        public async Task<bool> IsAccountPassedAllCourseMaterials(int accountId, int courseId)
        {
            var courseMaterialsList = await this.courseMaterialRepository.GetPageAsync<int>(
                c => c.CourseId == courseId,
                c => c.MaterialId,
                0, 
                await this.courseMaterialRepository.CountAsync(c => c.CourseId == courseId));
            return courseMaterialsList.ToList().TrueForAll(
                id => this.accountMaterialRepository.AnyAsync(
                    am => am.AccountId == accountId 
                    && am.MaterialId == id).Result);
        }
    }
}
