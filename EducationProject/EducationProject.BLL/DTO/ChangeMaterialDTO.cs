using System;
using System.Collections.Generic;
using System.Text;

using MaterialType = EducationProject.Core.Models.Enums.MaterialType;

namespace EducationProject.BLL.DTO
{
    public abstract class ChangeMaterialDTO : AccountIdBasedDTO
    {
       public MaterialDTO Material { get; set; }
    }
}
