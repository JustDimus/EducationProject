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
using EducationProject.Infrastructure.BLL.Mappings;

namespace EducationProject.Infrastructure.BLL.Services
{
    public class MaterialService : BaseService, IMaterialService
    {
        private IRepository<BaseMaterial> materialRepository;

        private IRepository<CourseMaterial> courseMaterialRepository;

        private IRepository<AccountMaterial> accountMaterialRepository;

        private IMapping<BaseMaterial, MaterialDTO> materialMapping;

        private ServiceResultMessageCollection serviceResultMessages;

        public MaterialService(
            IRepository<BaseMaterial> materialRepository,
            MaterialMapping materialMapping,
            IRepository<CourseMaterial> courseMaterialRepository,
            IRepository<AccountMaterial> accountMaterialRepository,
            ServiceResultMessageCollection serviceResultMessageCollection)
        {
            this.materialRepository = materialRepository;

            this.materialMapping = materialMapping;

            this.courseMaterialRepository = courseMaterialRepository;

            this.accountMaterialRepository = accountMaterialRepository;

            this.serviceResultMessages = serviceResultMessageCollection;
        }

        public async Task<IServiceResult> CreateAsync(ChangeEntityDTO<MaterialDTO> createEntity)
        {
            var newMaterial = this.materialMapping.Map(createEntity.Entity);

            await this.materialRepository.CreateAsync(newMaterial);

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult<IEnumerable<MaterialDTO>>> GetAsync(PageInfoDTO pageInfo)
        {
            return new ServiceResult<IEnumerable<MaterialDTO>>()
            {
                IsSuccessful = true,
                Result = await this.materialRepository.GetPageAsync<MaterialDTO>(
                    m => true,
                    this.materialMapping.ConvertExpression, 
                    pageInfo.PageNumber, 
                    pageInfo.PageSize)
            };
        }

        public async Task<IServiceResult> DeleteAsync(ChangeEntityDTO<MaterialDTO> deleteEntity)
        {
            await this.materialRepository.DeleteAsync(this.materialMapping.Map(deleteEntity.Entity));

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult<MaterialDTO>> GetMaterialInfoAsync(int id)
        {
            return new ServiceResult<MaterialDTO>()
            {
                IsSuccessful = true,
                Result = await this.materialRepository.GetAsync<MaterialDTO>(
                    m => m.Id == id,
                    this.materialMapping.ConvertExpression)
            };
        }

        public async Task<IServiceResult<bool>> IsExistAsync(MaterialDTO entity)
        {
            return new ServiceResult<bool>()
            {
                IsSuccessful = true,
                Result = await this.materialRepository.AnyAsync(m => m.Id == entity.Id)
            };
        }

        public async Task<IServiceResult> UpdateAsync(ChangeEntityDTO<MaterialDTO> changeEntity)
        {
            var isMaterialExist = await this.materialRepository.AnyAsync(m =>
                m.Id == changeEntity.Entity.Id);

            if (!isMaterialExist)
            {
                return this.GetDefaultActionResult(false);
            }

            await this.materialRepository.UpdateAsync(this.materialMapping.Map(changeEntity.Entity));

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult<IEnumerable<MaterialDTO>>> GetAllCourseMaterialsAsync(int courseId)
        {
            var i = await this.courseMaterialRepository.GetPageAsync<int>(
                cm => cm.CourseId == courseId,
                cm => cm.MaterialId, 
                0, 
                await this.courseMaterialRepository.CountAsync(cm => cm.CourseId == courseId));

            return new ServiceResult<IEnumerable<MaterialDTO>>()
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
