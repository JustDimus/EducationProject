using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL.EF
{
    public class AccountSkillDBO
    {
        public int AccountId { get; set; }

        public AccountDBO Account { get; set; }

        public int SkillId { get; set; }

        public SkillDBO Skill { get; set; }

        public int CurrentResult { get; set; }
    }
}
