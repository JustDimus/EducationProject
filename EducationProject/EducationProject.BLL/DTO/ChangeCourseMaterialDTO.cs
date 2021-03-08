using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class ChangeCourseMaterialDTO : AccountIdBasedDTO
    {
        public int CourseId { get; set; }

        public int MaterialId { get; set; }
    }
}
