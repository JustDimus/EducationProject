using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core
{
    public class AccountCourse: BaseEntity
    {
        public int AccountId { get; set; }

        public int CourseId { get; set; }
    }
}
