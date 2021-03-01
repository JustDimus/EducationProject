using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IAccountService : IBusinessService<ShortAccountInfoDTO>
    {
        FullAccountInfoDTO GetAccountInfo(int id);

        FullAccountInfoDTO GetAccountInfo(string token);

        bool AddAccountCourse(ChangeAccountCourseDTO accountCourseChange);

        bool RemoveAccountCourse(ChangeAccountCourseDTO accountCourseChange);

        bool ChangeAccountCourseStatus(ChangeAccountCourseDTO accountCourseChange);

        bool AddAccountMaterial(ChangeAccountMaterialDTO accountMaterialChange);

        bool RemoveAccountMaterial(ChangeAccountMaterialDTO accountMaterialChange);
    }
}
