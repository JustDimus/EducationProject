using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL
{
    public class AccountSkills: BaseEntity
    {
        public int Skill { get; set; }

        public int CurrentResult { get; set; }

        public int Level { get; set; }
    }
}
