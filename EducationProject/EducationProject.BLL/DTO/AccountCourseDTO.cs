using EducationProject.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class AccountCourseDTO
    {
        public int CourseId { get; set; }

        public string Title { get; set; }

        public ProgressStatus Status { get; set; }
    }
}
