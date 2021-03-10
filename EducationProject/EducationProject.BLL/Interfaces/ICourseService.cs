using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.BLL.Interfaces
{
    public interface ICourseService
    {
        Task<IServiceResult<FullCourseInfoDTO>> GetFullCourseInfoAsync(
            int courseId,
            PageInfoDTO materialPageInfo,
            PageInfoDTO skillPageInfo);

        Task<IServiceResult> ChangeCourseVisibilityAsync(CourseVisibilityDTO visibilityParams);

        Task<IServiceResult> AddCourseMaterialAsync(ChangeCourseMaterialDTO courseMaterialChange);

        Task<IServiceResult> RemoveCourseMaterialAsync(ChangeCourseMaterialDTO courseMaterialChange);

        Task<IServiceResult> AddCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange);

        Task<IServiceResult> RemoveCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange);

        Task<IServiceResult> ChangeCourseSkillAsync(ChangeCourseSkillDTO courseSkillChange);

        Task<IServiceResult<bool>> IsCourseContainsMaterialAsync(ChangeCourseMaterialDTO courseMaterial);

        Task<IServiceResult<bool>> IsCourseContainsMaterialAsync(IEnumerable<ChangeCourseMaterialDTO> courseMaterials);

        Task<IServiceResult<IEnumerable<ShortCourseInfoDTO>>> GetCoursesByCreatorIdAsync(GetCoursesByCreatorDTO courseCreator);

        Task<IServiceResult<int>> CreateCourseAsync(ShortCourseInfoDTO course);

        Task<IServiceResult> UpdateCourseAsync(ShortCourseInfoDTO course);

        Task<IServiceResult> DeleteCourseAsync(int courseId);

        Task<IServiceResult<ShortCourseInfoDTO>> GetCourseInfoAsync(int courseId);

        Task<IServiceResult<EntityInfoPageDTO<ShortCourseInfoDTO>>> GetCoursePageAsync(PageInfoDTO pageInfo);

        Task<IServiceResult<CourseSkillDTO>> GetCourseSkillAsync(int courseId, int skillId);
    }
}
