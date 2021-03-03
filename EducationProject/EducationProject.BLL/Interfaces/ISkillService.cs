using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface ISkillService : IBusinessService<SkillDTO>
    {
        IEnumerable<AccountSkillDTO> GetAccountSkills(GetAccountSkillsDTO accountSkills);

        bool AddSkilsToAccountByCourseId(AddSkillsToAccountByCourseDTO changeSkills);
    }
}
