using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class ChangeCourseMaterialDTO : TokenBasedDTO
    {
        public int CourseId { get; set; }

        public int MaterialId { get; set; }
    }
}
