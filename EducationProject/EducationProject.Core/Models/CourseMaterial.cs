using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.Models
{
    public class CourseMaterial
    {
        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int MaterialId { get; set; }

        public BaseMaterial Material { get; set; }
    }
}
