using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core
{
    public class AccountSkills: BaseEntity
    {
        public Skill Skill { get; set; }

        public int CurrentResult { get; set; }

        public int Level { get; set; }
    }
}
