using System;
using System.Collections.Generic;
using System.Text;
using EducationProject.Core.DAL;

namespace EducationProject.Core.BLL
{
    public class AccountSkill: BaseEntity
    {
        public Skill Skill { get; set; }

        public int CurrentResult { get; set; }

        public int Level { get; set; }
    }
}
