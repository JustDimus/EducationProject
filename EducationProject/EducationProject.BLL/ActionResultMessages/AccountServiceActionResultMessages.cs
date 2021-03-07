using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.ActionResultMessages
{
    public class AccountServiceActionResultMessages
    {
        public string AccountExist { get; private set; }

        public string AccountNotExist { get; private set; }

        public string AccountOrCourseNotExist { get; private set; }

        public string AccountCourseExist { get; private set; }

        public string AccountCourseNotExist { get; private set; }

        public string AccountOrSkillNotExist { get; private set; }

        public string AccountSkillExist { get; private set; }

        public string AccountNotPassedCourseMaterials { get; private set; }

        public string AccountMaterialExist { get; private set; }

        public string AccountMaterialNotExist { get; private set; }

        public string AccountOrMaterialNotExist { get; private set; }

        public AccountServiceActionResultMessages()
        {

        }
    }
}
