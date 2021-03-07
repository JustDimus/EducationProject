using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.BLL.Interfaces
{
    public interface ICourseService : IBusinessService<ShortCourseInfoDTO>
    {
        Task<IServiceResult<FullCourseInfoDTO>> GetCourseInfoAsync(int id);

        Task<IServiceResult> ChangeCourseVisibilityAsync(CourseVisibilityDTO visibilityParams);

        Task<IServiceResult> AddCourseMaterialAsync(ChangeCourseMaterialDTO courseMaterialChange);

        Task<IServiceResult> RemoveCourseMaterialAsync(ChangeCourseMaterialDTO courseMaterialChange);

        Task<IServiceResult> AddCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange);

        Task<IServiceResult> RemoveCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange);

        Task<IServiceResult> ChangeCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange);

        Task<IServiceResult<bool>> IsCourseContainsMaterialAsync(ChangeCourseMaterialDTO courseMaterial);

        Task<IServiceResult<bool>> IsCourseContainsMaterialAsync(IEnumerable<ChangeCourseMaterialDTO> courseMaterials);

        Task<IServiceResult<IEnumerable<ShortCourseInfoDTO>>> GetCoursesByCreatorIdAsync(GetCoursesByCreatorDTO courseCreator);
    }
}
