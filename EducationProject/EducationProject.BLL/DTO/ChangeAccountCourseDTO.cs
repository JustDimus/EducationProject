using EducationProject.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class ChangeAccountCourseDTO : AccountIdBasedDTO
    {
        public int CourseId { get; set; }

        public ProgressStatus? Status { get; set; }
    }
}
