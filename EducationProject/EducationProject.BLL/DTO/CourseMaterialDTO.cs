using EducationProject.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class CourseMaterialDTO
    {
        public int MaterialId { get; set; }

        public string MaterialTitle { get; set; }

        public MaterialType MaterialType { get; set; }

        public bool IsAccountPassed { get; set; }
    }
}
