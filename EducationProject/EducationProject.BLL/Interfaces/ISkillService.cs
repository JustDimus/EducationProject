using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.BLL.Interfaces
{
    public interface ISkillService// : IBusinessService<SkillDTO>
    {
        Task<IServiceResult<IEnumerable<AccountSkillDTO>>> GetAccountSkillsAsync(GetAccountSkillsDTO accountSkills);

        Task<IServiceResult> AddSkilsToAccountByCourseIdAsync(AddSkillsToAccountByCourseDTO changeSkills);

        Task<IServiceResult> CreateSkillAsync(SkillDTO skill);

        Task<IServiceResult<SkillDTO>> GetSkillAsync(int skillId);

        Task<IServiceResult> UpdateSkillAsync(SkillDTO skill);

        Task<IServiceResult> DeleteSkillAsync(int id);
    }
}
