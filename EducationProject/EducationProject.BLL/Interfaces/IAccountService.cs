using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.BLL.Interfaces
{
    public interface IAccountService// : IBusinessService<ShortAccountInfoDTO>
    {
        Task<IServiceResult> AddAccountCourseAsync(ChangeAccountCourseDTO accountCourseChange);

        Task<IServiceResult> RemoveAccountCourseAsync(ChangeAccountCourseDTO accountCourseChange);

        Task<IServiceResult> ChangeAccountCourseStatusAsync(ChangeAccountCourseDTO accountCourseChange);

        Task<IServiceResult> AddAccountMaterialAsync(ChangeAccountMaterialDTO accountMaterialChange);

        Task<IServiceResult> RemoveAccountMaterialAsync(ChangeAccountMaterialDTO accountMaterialChange);

        Task<IServiceResult<int>> TryLogInAsync(LogInDTO logInModel);

        Task<IServiceResult<int>> TryRegisterAsync(RegisterDTO registerModel);

        Task<IServiceResult<AccountInfoDTO>> GetAccountInfoAsync();

        Task<IServiceResult<AccountInfoDTO>> GetAccountInfoAsync(int accountId);

        Task<IServiceResult> UpdateAccountAsync(AccountInfoDTO accountInfo);

        Task<IServiceResult> DeleteAccountAsync();
    }
}
