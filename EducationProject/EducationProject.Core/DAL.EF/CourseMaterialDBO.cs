using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL.EF
{
    public class CourseMaterialDBO
    {
        public int CourseId { get; set; }

        public CourseDBO Course { get; set; }

        public int MaterialId { get; set; }

        public BaseMaterialDBO Material { get; set; }
    }
}
