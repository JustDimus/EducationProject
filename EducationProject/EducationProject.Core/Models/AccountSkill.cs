using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.Models
{
    public class AccountSkill
    {
        public int AccountId { get; set; }

        public Account Account { get; set; }

        public int SkillId { get; set; }

        public Skill Skill { get; set; }

        public int CurrentResult { get; set; }
    }
}
