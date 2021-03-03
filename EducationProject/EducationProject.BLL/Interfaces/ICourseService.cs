using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface ICourseService : IBusinessService<ShortCourseInfoDTO>
    {
        FullCourseInfoDTO GetCourseInfo(int id);

        bool ChangeCourseVisibility(CourseVisibilityDTO visibilityParams);

        bool AddCourseMaterial(ChangeCourseMaterialDTO courseMaterialChange);

        bool RemoveCourseMaterial(ChangeCourseMaterialDTO courseMaterialChange);

        bool AddCourseSkill(ChangeCourseSkillDTO courseSkillChange);

        bool RemoveCourseSkill(ChangeCourseSkillDTO courseSkillChange);

        bool ChangeCourseSkill(ChangeCourseSkillDTO courseSkillChange);

        bool IsCourseContainsMaterial(ChangeCourseMaterialDTO courseMaterial);

        bool IsCourseContainsMaterial(IEnumerable<ChangeCourseMaterialDTO> courseMaterials);

        IEnumerable<ShortCourseInfoDTO> GetCoursesByCreatorId(GetCoursesByCreator courseCreator);

        IEnumerable<ShortCourseInfoDTO> GetMyCourses(GetCoursesByCreator courseCreator);

        IEnumerable<int> GetAllCourseMaterialsId(int courseId);
    }
}
