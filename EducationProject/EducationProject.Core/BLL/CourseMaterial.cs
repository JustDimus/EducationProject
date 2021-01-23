using System;
using System.Collections.Generic;
using System.Text;
using EducationProject.Core.DAL;

namespace EducationProject.Core.BLL
{
    public class CourseMaterial: BaseEntity
    {
        public Material Material { get; set; }

        public int Position { get; set; }
    }
}
