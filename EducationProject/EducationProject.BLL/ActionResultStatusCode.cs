using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL
{
    public enum StatusCode
    {
        NoError,
        AccountNotExist,
        CourseNotExist,
        MaterialNotExist,
        SkillNotExist,
        AccountAlreadyExist,
        CourseAlreadyExist,
        MaterialAlreadyExist,
        SkillAlreadyExist,
        AccountOrCourseNotExist,
        AccountOrMaterialNotExist,
        AccountOrSkillNotExist,
        CourseOrMaterialNotExist,
        CourseOrSkillNotExist,
        AccountCourseAlreadyExist,
        AccountMaterialAlreadyExist,
        CourseMaterialAlreadyExist,
        CourseSkillAlreadyExist,
        AccountCourseNotExist,
        AccountMaterialNotExist,
        CourseMaterialNotExist,
        CourseSkillNotExist,
        AccountDidNotPassCourseMaterials,
        AccountCanNotChangeCourse
    }
}
