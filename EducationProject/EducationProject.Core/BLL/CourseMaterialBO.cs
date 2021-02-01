using System;
using System.Collections.Generic;
using System.Text;
using EducationProject.Core.DAL;

namespace EducationProject.Core.BLL
{
    public class CourseMaterialBO: BaseEntity
    {
        public BaseMaterial Material { get; set; }

        public int Position { get; set; }
    }
}
