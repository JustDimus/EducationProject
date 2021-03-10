using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL
{
    public class ServiceResultMessageCollection
    {
        public string LogInError => "LogInError";

        public string EmailExist => "EmailExist";

        public string ConnectionFailed => "ConnectionFailed";

        public string PermissionDenied => "PermissionDenied";

        public string AccountNotExist => "AccountExist";

        public string SkillTitleExist => "SkillTitleExist";

        public string SkillNotExist => "SkillNotExist";

        public string CourseTitleExist => "CourseTitleExist";

        public string CourseNotExist => "CourseNotExist";

        public string MaterialNotExist => "MaterialNotExist";

        public string CourseOrSkillNotExist => "CourseOrSkillNotExist";

        public string CourseOrMaterialNotExist => "CourseOrMaterialNotExist";

        public string CourseSkillExist => "CourseSkillExist";

        public string CourseMaterialExist => "CourseMaterialExist";

        public string CourseSkillNotExist => "CourseSkillNotExist";

        public string CourseMaterialNotExist => "CourseMaterialNotExist";

        public string ZeroCourseMaterials => "ZeroCourseMaterials";
    }
}
