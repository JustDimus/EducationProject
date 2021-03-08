using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.ActionResultMessages
{
    public class CourseServiceActionResultMessages
    {
        public string CourseOrMaterialNotExist { get; private set; }

        public string CourseMaterialNotExist { get; private set; }

        public string CourseMaterialExist { get; private set; }

        public string AccountCanNotChangeCourse { get; private set; }

        public string CourseOrSkillNotExist { get; private set; }

        public string CourseSkillExist { get; private set; }

        public string CourseSkillNotExist { get; private set; }

        public string CourseNotExist { get; private set; }

        public string CourseExist { get; private set; }

        public string CourseHasNoMaterials { get; private set; }
    }
}
