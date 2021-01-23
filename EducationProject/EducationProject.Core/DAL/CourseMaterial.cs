using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL
{
    public class CourseMaterial: BaseEntity
    {
        public int MaterialId { get; set; }

        public int CourseId { get; set; }

        public int Position { get; set; }
    }
}
