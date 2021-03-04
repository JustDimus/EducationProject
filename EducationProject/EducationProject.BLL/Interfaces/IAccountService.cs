using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.BLL.Interfaces
{
    public interface IAccountService : IBusinessService<ShortAccountInfoDTO>
    {
        Task<IActionResult<FullAccountInfoDTO>> GetAccountInfoAsync(int id);

        Task<IActionResult> AddAccountCourseAsync(ChangeAccountCourseDTO accountCourseChange);

        Task<IActionResult> RemoveAccountCourseAsync(ChangeAccountCourseDTO accountCourseChange);

        Task<IActionResult> ChangeAccountCourseStatusAsync(ChangeAccountCourseDTO accountCourseChange);

        Task<IActionResult> AddAccountMaterialAsync(ChangeAccountMaterialDTO accountMaterialChange);

        Task<IActionResult> RemoveAccountMaterialAsync(ChangeAccountMaterialDTO accountMaterialChange);
    }
}
