using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.BLL.Interfaces
{
    public interface IMaterialService
    {
        Task<IServiceResult<IEnumerable<MaterialDTO>>> GetAllCourseMaterialsAsync(int courseId);

        Task<bool> IsAccountPassedAllCourseMaterialsAsync(int accountId, int courseId);

        Task<IServiceResult<int>> CreateMaterialAsync(MaterialDTO material);

        Task<IServiceResult> UpdateMaterialAsync(MaterialDTO material);

        Task<IServiceResult> DeleteMaterialAsync(int materialId);

        Task<IServiceResult<MaterialDTO>> GetMaterialAsync(int materialId);

        Task<IServiceResult<EntityInfoPageDTO<MaterialDTO>>> GetMaterialPageAsync(PageInfoDTO pageInfo);

        Task<bool> IsExistAsync(int materialId);

        Task<IServiceResult<EntityInfoPageDTO<CourseMaterialDTO>>> GetCourseMaterialPageAsync(
            int courseId,
            int accountId,
            PageInfoDTO pageInfo);
    }
}
