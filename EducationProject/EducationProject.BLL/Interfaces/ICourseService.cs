using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.BLL.Interfaces
{
    public interface ICourseService : IBusinessService<ShortCourseInfoDTO>
    {
        Task<IActionResult<FullCourseInfoDTO>> GetCourseInfoAsync(int id);

        Task<IActionResult> ChangeCourseVisibilityAsync(CourseVisibilityDTO visibilityParams);

        Task<IActionResult> AddCourseMaterialAsync(ChangeCourseMaterialDTO courseMaterialChange);

        Task<IActionResult> RemoveCourseMaterialAsync(ChangeCourseMaterialDTO courseMaterialChange);

        Task<IActionResult> AddCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange);

        Task<IActionResult> RemoveCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange);

        Task<IActionResult> ChangeCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange);

        Task<IActionResult<bool>> IsCourseContainsMaterialAsync(ChangeCourseMaterialDTO courseMaterial);

        Task<IActionResult<bool>> IsCourseContainsMaterialAsync(IEnumerable<ChangeCourseMaterialDTO> courseMaterials);

        Task<IActionResult<IEnumerable<ShortCourseInfoDTO>>> GetCoursesByCreatorIdAsync(GetCoursesByCreator courseCreator);

        Task<IActionResult<IEnumerable<int>>> GetAllCourseMaterialsIdAsync(int courseId);
    }
}
