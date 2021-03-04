using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class CourseVisibilityDTO : AccountIdBasedDTO
    {
        public int CourseId { get; set; }

        public bool Visibility { get; set; }
    }
}
