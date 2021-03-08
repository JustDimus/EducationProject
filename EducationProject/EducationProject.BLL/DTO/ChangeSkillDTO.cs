using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class ChangeSkillDTO : AccountIdBasedDTO
    {
        public SkillDTO Skill { get; set; }
    }
}
