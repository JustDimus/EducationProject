using EducationProject.Core.DAL.EF.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class ChangeAccountCourseDTO : TokenBasedDTO
    {
        public int AccountId { get; set; }

        public int CourseId { get; set; }

        public ProgressStatus? Status { get; set; }
    }
}
