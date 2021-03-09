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
using System.Linq.Expressions;

namespace EducationProject.Infrastructure.BLL.Services
{
    public class MaterialService : BaseService, IMaterialService
    {
        private IRepository<BaseMaterial> materialRepository;

        private IRepository<CourseMaterial> courseMaterialRepository;

        private IRepository<AccountMaterial> accountMaterialRepository;

        private IAuthorizationService authorizationService;

        private MaterialMapping materialMapping;

        private ServiceResultMessageCollection serviceResultMessages;

        public MaterialService(
            IRepository<BaseMaterial> materialRepository,
            MaterialMapping materialMapping,
            IRepository<CourseMaterial> courseMaterialRepository,
            IRepository<AccountMaterial> accountMaterialRepository,
            IAuthorizationService authorizationService,
            ServiceResultMessageCollection serviceResultMessageCollection)
        {
            this.materialRepository = materialRepository;

            this.materialMapping = materialMapping;

            this.authorizationService = authorizationService;

            this.courseMaterialRepository = courseMaterialRepository;

            this.accountMaterialRepository = accountMaterialRepository;

            this.serviceResultMessages = serviceResultMessageCollection;
        }

        public async Task<IServiceResult<EntityInfoPageDTO<CourseMaterialDTO>>> GetCourseMaterialPageAsync(
            int courseId, 
            int accountId, 
            PageInfoDTO pageInfo)
        {
            try
            {
                var pageCount = await this.GetCourseMaterialPagesCount(
                    pageInfo.PageSize,
                    cm => cm.CourseId == courseId);

                if (pageInfo.PageNumber >= pageCount || pageInfo.PageNumber < 0)
                {
                    pageInfo.PageNumber = 0;
                }


                var courseMaterialInfoPage = new EntityInfoPageDTO<CourseMaterialDTO>()
                {
                    CurrentPage = pageInfo.PageNumber,
                    CurrentPageSize = pageInfo.PageSize
                };

                courseMaterialInfoPage.CanMoveBack = pageInfo.PageNumber > 0;
                courseMaterialInfoPage.CanMoveForward = pageCount > pageInfo.PageNumber + 1;

                var courseMaterials = await this.courseMaterialRepository.GetPageAsync<CourseMaterialDTO>(
                    cm => cm.CourseId == courseId,
                    this.materialMapping.CourseMaterialDTOExpression,
                    pageInfo.PageNumber,
                    pageInfo.PageSize);

                foreach(var material in courseMaterials)
                {
                    material.IsAccountPassed = await this.accountMaterialRepository.AnyAsync(
                        am => am.MaterialId == material.MaterialId
                        && am.AccountId == accountId);
                }

                courseMaterialInfoPage.Entities = courseMaterials;

                return new ServiceResult<EntityInfoPageDTO<CourseMaterialDTO>>()
                {
                    IsSuccessful = true,
                    Result = courseMaterialInfoPage
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<EntityInfoPageDTO<CourseMaterialDTO>>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult<int>> CreateMaterialAsync(MaterialDTO material)
        {
            try
            {
                var newMaterial = this.materialMapping.Map(material);

                await this.materialRepository.CreateAsync(newMaterial);

                await this.materialRepository.SaveAsync();

                return new ServiceResult<int>()
                {
                    IsSuccessful = true,
                    Result = newMaterial.Id
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<int>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult> UpdateMaterialAsync(MaterialDTO material)
        {
            try
            {
                var isMaterialExist = await this.materialRepository.AnyAsync(m =>
                m.Id == material.Id);

                if (!isMaterialExist)
                {
                    return this.GetDefaultActionResult(
                        false,
                        this.serviceResultMessages.MaterialNotExist);
                }

                await this.materialRepository.UpdateAsync(this.materialMapping.Map(material));

                await this.materialRepository.SaveAsync();

                return this.GetDefaultActionResult(true);
            }
            catch(Exception ex)
            {
                return new ServiceResult<int>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult> DeleteMaterialAsync(int materialId)
        {
            try
            {
                await this.materialRepository.DeleteAsync(m => m.Id == materialId);

                return this.GetDefaultActionResult(true);
            }
            catch(Exception ex)
            {
                return ServiceResult.GetDefault(
                    false,
                    ex.Message);
            }
        }

        public async Task<IServiceResult<MaterialDTO>> GetMaterialAsync(int materialId)
        {
            try
            {
                return new ServiceResult<MaterialDTO>()
                {
                    IsSuccessful = true,
                    Result = await this.materialRepository.GetAsync<MaterialDTO>(
                    m => m.Id == materialId,
                    this.materialMapping.ConvertExpression)
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<MaterialDTO>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult<EntityInfoPageDTO<MaterialDTO>>> GetMaterialPageAsync(PageInfoDTO pageInfo)
        {
            try
            {
                var pageCount = await this.GetPagesCountAsync(pageInfo.PageSize, t => true);

                if (pageInfo.PageNumber >= pageCount || pageInfo.PageNumber < 0)
                {
                    pageInfo.PageNumber = 0;
                }

                var materialInfoPage = new EntityInfoPageDTO<MaterialDTO>()
                {
                    CurrentPage = pageInfo.PageNumber,
                    CurrentPageSize = pageInfo.PageSize
                };

                materialInfoPage.CanMoveBack = pageInfo.PageNumber > 0;
                materialInfoPage.CanMoveForward = pageCount > pageInfo.PageNumber + 1;

                materialInfoPage.Entities = await this.materialRepository.GetPageAsync<MaterialDTO>(
                    s => true,
                    this.materialMapping.ConvertExpression,
                    pageInfo.PageNumber,
                    pageInfo.PageSize);

                return new ServiceResult<EntityInfoPageDTO<MaterialDTO>>()
                {
                    IsSuccessful = true,
                    Result = materialInfoPage
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<EntityInfoPageDTO<MaterialDTO>>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<bool> IsExistAsync(MaterialDTO material)
        {
            try
            {
                return await this.materialRepository.AnyAsync(m => m.Id == material.Id);
            }
            catch(Exception)
            {
                return false;
            }   
        }

        public async Task<IServiceResult<IEnumerable<MaterialDTO>>> GetAllCourseMaterialsAsync(int courseId)
        {
            try
            {
                return new ServiceResult<IEnumerable<MaterialDTO>>()
                {
                    IsSuccessful = true,
                    Result = await this.courseMaterialRepository.GetPageAsync<MaterialDTO>(
                        cm => cm.CourseId == courseId,
                        this.materialMapping.CourseMaterialExpression,
                        0,
                        await this.courseMaterialRepository.CountAsync(cm => cm.CourseId == courseId))
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<IEnumerable<MaterialDTO>>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<bool> IsAccountPassedAllCourseMaterialsAsync(int accountId, int courseId)
        {
            try
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
            catch(Exception)
            {
                return false;
            }
        }

        private async Task<int> GetPagesCountAsync(int pageSize, Expression<Func<BaseMaterial, bool>> materialCondition)
        {
            var result = await this.materialRepository.CountAsync(materialCondition);

            if (result % pageSize == 0)
            {
                result = result / pageSize;
            }
            else
            {
                result = (result / pageSize) + 1;
            }

            return result;
        }
    
        private async Task<int> GetCourseMaterialPagesCount(int pageSize, Expression<Func<CourseMaterial, bool>> condition)
        {
            var result = await this.courseMaterialRepository.CountAsync(condition);

            if (result % pageSize == 0)
            {
                result = result / pageSize;
            }
            else
            {
                result = (result / pageSize) + 1;
            }

            return result;
        }
    }
}
