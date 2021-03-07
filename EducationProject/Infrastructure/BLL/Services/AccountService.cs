using EducationProject.BLL.Interfaces;
using EducationProject.Core.Models;
using System;
using System.Collections.Generic;
using EducationProject.BLL.DTO;
using EducationProject.DAL.Interfaces;
using System.Threading.Tasks;
using EducationProject.BLL;

using CourseStatus = EducationProject.Core.Models.Enums.ProgressStatus;
using EducationProject.BLL.ActionResultMessages;

namespace EducationProject.Infrastructure.BLL.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private IRepository<AccountCourse> accountCourseRepository;

        private IRepository<AccountMaterial> accountMaterialRepository;

        private IRepository<Account> accountRepository;

        private ICourseService courseService;

        private IMaterialService materialService;

        private ISkillService skillService;

        private IMapping<Account, ShortAccountInfoDTO> accountMapping;

        private AccountServiceActionResultMessages accountResultMessages;

        private int defaultAccountInfoPageSize;

        public AccountService(
            IRepository<Account> accountRepository,
            IRepository<AccountCourse> accountCoursesRepository,
            IRepository<AccountMaterial> accountMaterialsRepository,
            ICourseService courseService,
            IMaterialService materialService,
            ISkillService skillService,
            IMapping<Account, ShortAccountInfoDTO> accountMapping,
            AccountServiceActionResultMessages accountActionResultMessages,
            int accountInfoPageSize)
        {
            this.accountCourseRepository = accountCoursesRepository;
            this.accountMaterialRepository = accountMaterialsRepository;
            this.accountRepository = accountRepository;

            this.courseService = courseService;
            this.skillService = skillService;
            this.materialService = materialService;

            this.accountMapping = accountMapping;

            this.accountResultMessages = accountActionResultMessages;

            this.defaultAccountInfoPageSize = accountInfoPageSize;
        }

        public async Task<IServiceResult> CreateAsync(ChangeEntityDTO<ShortAccountInfoDTO> createEntity)
        {
            var isEmailAlreadyExist = await this.accountRepository.AnyAsync(a =>
                a.Email == createEntity.Entity.Email);

            if (isEmailAlreadyExist)
            {
                return this.GetDefaultActionResult(false, this.accountResultMessages.AccountExist);
            }

            var account = this.accountMapping.Map(createEntity.Entity);

            account.RegistrationDate = DateTime.Now;

            await this.accountRepository.CreateAsync(account);

            await this.accountRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult> UpdateAsync(ChangeEntityDTO<ShortAccountInfoDTO> updateEntity)
        {
            var isAccountExist = await this.accountRepository.AnyAsync(a =>
                a.Id == updateEntity.Entity.Id);

            if (!isAccountExist)
            {
                return this.GetDefaultActionResult(false, this.accountResultMessages.AccountNotExist);
            }

            await this.accountRepository.UpdateAsync(this.accountMapping.Map(updateEntity.Entity));

            await this.accountRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult> DeleteAsync(ChangeEntityDTO<ShortAccountInfoDTO> deleteEntity)
        {
            await this.accountRepository.DeleteAsync(this.accountMapping.Map(deleteEntity.Entity));

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult<bool>> IsExistAsync(ShortAccountInfoDTO checkEntity)
        {
            return new ServiceResult<bool>()
            {
                IsSuccessful = true,
                Result = await this.accountRepository.AnyAsync(a => a.Id == checkEntity.Id)
            };
        }

        public async Task<IServiceResult<IEnumerable<ShortAccountInfoDTO>>> GetAsync(PageInfoDTO pageInfo)
        {
            return new ServiceResult<IEnumerable<ShortAccountInfoDTO>>()
            {
                IsSuccessful = true,
                Result = await this.accountRepository.GetPageAsync<ShortAccountInfoDTO>(
                    a => true,
                    this.accountMapping.ConvertExpression,
                    pageInfo.PageNumber, 
                    pageInfo.PageSize)
            };
        }

        public async Task<IServiceResult> AddAccountCourseAsync(ChangeAccountCourseDTO accountCourseChange)
        {
            var isAccountAndCourseExist = await this.ValidateAccountCourseAsync(accountCourseChange);

            if (!isAccountAndCourseExist)
            {
                return this.GetDefaultActionResult(false, this.accountResultMessages.AccountOrCourseNotExist);
            }

            var isAccountCourseAlreadyExist = await this.accountCourseRepository.AnyAsync(ac =>
                ac.CourseId == accountCourseChange.CourseId
                && ac.AccountId == accountCourseChange.AccountId);

            if (isAccountCourseAlreadyExist)
            {
                return this.GetDefaultActionResult(false, this.accountResultMessages.AccountCourseExist);
            }

            await this.accountCourseRepository.CreateAsync(new AccountCourse()
            {
                AccountId = accountCourseChange.AccountId,
                CourseId = accountCourseChange.CourseId,
                Status = CourseStatus.InProgress
            });

            await this.accountCourseRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult> RemoveAccountCourseAsync(ChangeAccountCourseDTO accountCourseChange)
        {
            var isAccountAndCourseExist = await this.ValidateAccountCourseAsync(accountCourseChange);

            if (!isAccountAndCourseExist)
            {
                return this.GetDefaultActionResult(false, this.accountResultMessages.AccountCourseNotExist);
            }

            await this.accountCourseRepository.DeleteAsync(new AccountCourse()
            {
                AccountId = accountCourseChange.AccountId,
                CourseId = accountCourseChange.CourseId
            });

            await this.accountCourseRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult> ChangeAccountCourseStatusAsync(ChangeAccountCourseDTO accountCourseChange)
        {
            var isAccountAndCourseExist = await this.ValidateAccountCourseAsync(accountCourseChange);

            if (!isAccountAndCourseExist)
            {
                return this.GetDefaultActionResult(false, this.accountResultMessages.AccountOrCourseNotExist);
            }

            var isAccountCourseExist = await this.accountCourseRepository.AnyAsync(ac =>
                (ac.CourseId == accountCourseChange.CourseId && ac.AccountId == accountCourseChange.AccountId)
                && (ac.CourseId == accountCourseChange.CourseId && ac.AccountId == accountCourseChange.AccountId
                && ac.Status != accountCourseChange.Status));

            if (!isAccountCourseExist)
            {
                return this.GetDefaultActionResult(false, this.accountResultMessages.AccountCourseNotExist);
            }

            if (accountCourseChange.Status == CourseStatus.Passed)
            {
                var isAccountPassedAllCourseMaterials = await this.materialService
                    .IsAccountPassedAllCourseMaterials(
                    accountCourseChange.AccountId,
                    accountCourseChange.CourseId);

                if (!isAccountPassedAllCourseMaterials)
                {
                    return this.GetDefaultActionResult(false, this.accountResultMessages.AccountNotPassedCourseMaterials);
                }
            }

            var accountCourse = await this.accountCourseRepository.GetAsync(ac =>
                ac.AccountId == accountCourseChange.AccountId
                && ac.CourseId == accountCourseChange.CourseId);

            if (!accountCourse.OncePassed && accountCourseChange.Status == CourseStatus.Passed)
            {
                accountCourse.OncePassed = true;

                await this.skillService.AddSkilsToAccountByCourseIdAsync(new AddSkillsToAccountByCourseDTO()
                {
                    AccountId = accountCourseChange.AccountId,
                    CourseId = accountCourseChange.CourseId
                });
            }

            accountCourse.Status = accountCourseChange.Status.Value;

            await this.accountCourseRepository.UpdateAsync(accountCourse);

            await this.accountCourseRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult> AddAccountMaterialAsync(ChangeAccountMaterialDTO accountMaterialChange)
        {
            var isAccountAndMaterialExist = await this.ValidateAccountMaterialAsync(accountMaterialChange);

            if (!isAccountAndMaterialExist)
            {
                return this.GetDefaultActionResult(false, this.accountResultMessages.AccountOrMaterialNotExist);
            }

            var isAccountMaterialAlreadyExist = await this.accountMaterialRepository.AnyAsync(am =>
                am.AccountId == accountMaterialChange.AccountId
                && am.MaterialId == accountMaterialChange.MaterialId);

            if (isAccountMaterialAlreadyExist)
            {
                return this.GetDefaultActionResult(false, this.accountResultMessages.AccountMaterialExist);
            }

            await this.accountMaterialRepository.CreateAsync(new AccountMaterial()
            {
                AccountId = accountMaterialChange.AccountId,
                MaterialId = accountMaterialChange.MaterialId
            });

            await this.accountMaterialRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult> RemoveAccountMaterialAsync(ChangeAccountMaterialDTO accountMaterialChange)
        {
            var isAccountAndMaterialExist = await this.ValidateAccountMaterialAsync(accountMaterialChange);

            if (!isAccountAndMaterialExist)
            {
                return this.GetDefaultActionResult(false, this.accountResultMessages.AccountMaterialNotExist);
            }

            await this.accountMaterialRepository.DeleteAsync(new AccountMaterial()
            {
                AccountId = accountMaterialChange.AccountId,
                MaterialId = accountMaterialChange.MaterialId
            });

            await this.accountMaterialRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult<FullAccountInfoDTO>> GetAccountInfoAsync(int accountId)
        {
            var result = await this.accountRepository.GetAsync<FullAccountInfoDTO>(
                a => a.Id == accountId,
                a => new FullAccountInfoDTO()
                {
                    Id = a.Id,
                    RegistrationDate = a.RegistrationDate,
                    Email = a.Email,
                    Password = null,
                    SecondName = a.SecondName,
                    FirstName = a.FirstName,
                    PhoneNumber = a.PhoneNumber
                });

            result.PassedCoursesCount = await this.accountCourseRepository.CountAsync(ac =>
                ac.AccountId == accountId && ac.Status == CourseStatus.Passed);

            result.PassedCourses = await this.accountCourseRepository.GetPageAsync<AccountCourseDTO>(
                ac => ac.AccountId == accountId && ac.Status == CourseStatus.Passed,
                ac => new AccountCourseDTO()
                {
                    Title = ac.Course.Title,
                    CourseId = ac.CourseId,
                    Status = CourseStatus.Passed,
                }, 
                0, 
                this.defaultAccountInfoPageSize);

            result.CoursesInProgressCount = await this.accountCourseRepository.CountAsync(ac => 
                ac.AccountId == accountId && ac.Status == CourseStatus.InProgress);

            result.CoursesInProgress = await this.accountCourseRepository.GetPageAsync<AccountCourseDTO>(
                ac => ac.AccountId == accountId && ac.Status == CourseStatus.InProgress,
                ac => new AccountCourseDTO()
                {
                    Title = ac.Course.Title,
                    CourseId = ac.CourseId,
                    Status = CourseStatus.InProgress
                }, 
                0,
                this.defaultAccountInfoPageSize);

            var accountSkillsResult = await this.skillService.GetAccountSkillsAsync(new GetAccountSkillsDTO()
            {
                AccountId = accountId,
                PageNumber = 0,
                PageSize = this.defaultAccountInfoPageSize
            });

            result.AccountSkills = accountSkillsResult.Result;

            return new ServiceResult<FullAccountInfoDTO>()
            {
                IsSuccessful = true,
                Result = result
            };
        }

        private async Task<bool> ValidateAccountMaterialAsync(ChangeAccountMaterialDTO changeAccountMaterial)
        {
            var isMaterialExist = await this.materialService.IsExistAsync(new MaterialDTO()
            {
                Id = changeAccountMaterial.MaterialId
            });

            var isAccountExist = await this.accountRepository.AnyAsync(a =>
                a.Id == changeAccountMaterial.AccountId);

            if (!isMaterialExist.Result || !isAccountExist)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateAccountCourseAsync(ChangeAccountCourseDTO changeAccountCourse)
        {
            var isCourseExist = await this.courseService.IsExistAsync(new ShortCourseInfoDTO()
            {
                Id = changeAccountCourse.CourseId
            });

            var isAccountExist = await this.accountRepository.AnyAsync(a =>
                a.Id == changeAccountCourse.AccountId);

            if (!isCourseExist.Result || !isAccountExist)
            {
                return false;
            }

            return true;
        }
    }
}
