using System;
using System.Collections.Generic;
using System.Text;

//NONEED

namespace EducationProject.Core.DAL
{
    public class AccountSkillDBO: BaseEntity
    {
        public int Skill { get; set; }

        public int CurrentResult { get; set; }

        public int Level { get; set; }
    }
}
