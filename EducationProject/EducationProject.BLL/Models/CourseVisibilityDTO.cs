using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class CourseVisibilityDTO : TokenBasedDTO
    {
        public int CourseId { get; set; }

        public bool Visibility { get; set; }
    }
}
