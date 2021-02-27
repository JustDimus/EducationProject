using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class ChangeSkillDTO : TokenBasedDTO
    {
        public SkillDTO Skill { get; set; }
    }
}
