using System;
using System.Collections.Generic;
using System.Text;

using MaterialType = EducationProject.Core.DAL.EF.Enums.MaterialType;

namespace EducationProject.BLL.Models
{
    public abstract class ChangeMaterialDTO : TokenBasedDTO
    {
       public MaterialDTO Material { get; set; }
    }
}
