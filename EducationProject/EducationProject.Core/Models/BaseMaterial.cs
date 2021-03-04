using EducationProject.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace EducationProject.Core.Models
{
    public abstract class BaseMaterial: BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public MaterialType Type { get; set; }

        public List<CourseMaterial> CourseMaterials { get; set; }

        public List<AccountMaterial> AccountMaterials { get; set; }
    }
}
