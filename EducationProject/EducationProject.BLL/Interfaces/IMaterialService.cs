using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.BLL.Interfaces
{
    public interface IMaterialService : IBusinessService<MaterialDTO>
    {
        Task<IServiceResult<MaterialDTO>> GetMaterialInfoAsync(int id);

        Task<IServiceResult<IEnumerable<MaterialDTO>>> GetAllCourseMaterialsAsync(int courseId);

        Task<bool> IsAccountPassedAllCourseMaterials(int accountId, int courseId);
    }
}
