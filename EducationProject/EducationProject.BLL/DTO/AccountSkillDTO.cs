using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class AccountSkillDTO
    {
        public int SkillId { get; set; }

        public string Title { get; set; }

        public int CurrentResult { get; set; }

        public int Level { get; set; }

        public int MaxValue { get; set; }
    }
}
