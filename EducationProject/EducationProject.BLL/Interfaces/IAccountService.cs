using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.BLL.Interfaces
{
    public interface IAccountService : IBusinessService<ShortAccountInfoDTO>
    {
        Task<IServiceResult<FullAccountInfoDTO>> GetAccountInfoAsync(int id);

        Task<IServiceResult> AddAccountCourseAsync(ChangeAccountCourseDTO accountCourseChange);

        Task<IServiceResult> RemoveAccountCourseAsync(ChangeAccountCourseDTO accountCourseChange);

        Task<IServiceResult> ChangeAccountCourseStatusAsync(ChangeAccountCourseDTO accountCourseChange);

        Task<IServiceResult> AddAccountMaterialAsync(ChangeAccountMaterialDTO accountMaterialChange);

        Task<IServiceResult> RemoveAccountMaterialAsync(ChangeAccountMaterialDTO accountMaterialChange);
    }
}
