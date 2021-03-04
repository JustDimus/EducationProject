using EducationProject.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.Models
{
    public class AccountCourse
    {
        public int AccountId { get; set; }

        public Account Account { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public ProgressStatus Status { get; set; }

        public bool OncePassed { get; set; }
    }
}
