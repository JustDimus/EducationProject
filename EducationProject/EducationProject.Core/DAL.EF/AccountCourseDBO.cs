using EducationProject.Core.DAL.EF.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL.EF
{
    public class AccountCourseDBO
    {
        public int AccountID { get; set; }

        public AccountDBO Account { get; set; }

        public int CourseId { get; set; }

        public CourseDBO Course { get; set; }

        public ProgressStatus Status { get; set; }
    }
}
