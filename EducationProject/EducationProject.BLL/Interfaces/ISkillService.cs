using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.BLL.Interfaces
{
    public interface ISkillService : IBusinessService<SkillDTO>
    {
        Task<IEnumerable<AccountSkillDTO>> GetAccountSkills(GetAccountSkillsDTO accountSkills);

        Task<bool> AddSkilsToAccountByCourseIdAsync(AddSkillsToAccountByCourseDTO changeSkills);
    }
}
