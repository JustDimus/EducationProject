using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.BLL.Interfaces
{
    public interface IMaterialService : IBusinessService<MaterialDTO>
    {
        Task<IActionResult<MaterialDTO>> GetMaterialInfo(int id);
    }
}
