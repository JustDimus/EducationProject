using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IMaterialService : IBusinessService<MaterialDTO>
    {
        MaterialDTO GetMaterialInfo(int id);
    }
}
