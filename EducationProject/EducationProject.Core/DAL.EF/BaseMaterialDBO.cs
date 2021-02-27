using EducationProject.Core.DAL.EF.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace EducationProject.Core.DAL.EF
{
    public abstract class BaseMaterialDBO: BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public MaterialType Type { get; set; }

        public List<CourseMaterialDBO> CourseMaterials { get; set; }

        public List<AccountMaterialDBO> AccountMaterials { get; set; }
    }
}
