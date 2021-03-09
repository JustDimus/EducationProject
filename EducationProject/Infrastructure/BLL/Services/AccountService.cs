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
using EducationProject.Infrastructure.BLL.Mappings;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace EducationProject.Infrastructure.BLL.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private IRepository<AccountCourse> accountCourseRepository;

        private IRepository<AccountMaterial> accountMaterialRepository;

        private IRepository<Account> accountRepository;

        private IMaterialService materialService;

        private ISkillService skillService;

        private IMapping<Account, ShortAccountInfoDTO> accountMapping;

        private ServiceResultMessageCollection serviceResultMessages;

        private IPasswordHasher passwordHasher;

        private IHttpContextAccessor httpContext;

        private int defaultAccountInfoPageSize;

        public AccountService(
            IRepository<Account> accountRepository,
            IRepository<AccountCourse> accountCoursesRepository,
            IRepository<AccountMaterial> accountMaterialsRepository,
            IMaterialService materialService,
            ISkillService skillService,
            AccountMapping accountMapping,
            ServiceResultMessageCollection serviceResultMessageCollection,
            IPasswordHasher passwordHasher,
            IHttpContextAccessor httpContextAccessor)
        {
            this.accountCourseRepository = accountCoursesRepository;
            this.accountMaterialRepository = accountMaterialsRepository;
            this.accountRepository = accountRepository;

            //this.courseService = courseService;
            this.skillService = skillService;
            this.materialService = materialService;

            this.accountMapping = accountMapping;

            this.serviceResultMessages = serviceResultMessageCollection;

            this.passwordHasher = passwordHasher;

            this.httpContext = httpContextAccessor;

            this.defaultAccountInfoPageSize = 10;
        }

        public async Task<IServiceResult<int>> TryLogInAsync(LogInDTO logInModel)
        {
            try
            {
                var accountId = await this.accountRepository.GetAsync<int>(
                    p => p.Email == logInModel.Email
                    && p.Password == this.passwordHasher.Hash(logInModel.Password),
                    p => p.Id);

                if (accountId == default)
                {
                    return new ServiceResult<int>()
                    {
                        IsSuccessful = false,
                        MessageCode = this.serviceResultMessages.LogInError
                    };
                }
                else
                {
                    return new ServiceResult<int>()
                    {
                        IsSuccessful = true,
                        Result = accountId
                    };
                }
            }
            catch(Exception ex)
            {
                return new ServiceResult<int>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult<int>> TryRegisterAsync(RegisterDTO registerModel)
        {
            try
            {
                var isAccountEmailExist = await this.accountRepository.AnyAsync(a => a.Email == registerModel.Email);

                if (isAccountEmailExist)
                {
                    return new ServiceResult<int>()
                    {
                        IsSuccessful = false,
                        MessageCode = this.serviceResultMessages.EmailExist
                    };
                }

                var newAccount = new Account()
                {
                    Email = registerModel.Email,
                    Password = this.passwordHasher.Hash(registerModel.Password)
                };

                await this.accountRepository.CreateAsync(newAccount);

                await this.accountRepository.SaveAsync();

                return new ServiceResult<int>()
                {
                    IsSuccessful = true,
                    Result = newAccount.Id
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<int>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult<AccountInfoDTO>> GetAccountInfoAsync()
        {
            var accountId = this.GetAccountId();

            return await this.GetAccountInfoAsync(accountId);
        }

        public async Task<IServiceResult<AccountInfoDTO>> GetAccountInfoAsync(int accountId)
        {
            try
            {
                return new ServiceResult<AccountInfoDTO>()
                {
                    IsSuccessful = true,
                    Result = await this.accountRepository.GetAsync<AccountInfoDTO>(
                    p => p.Id == accountId,
                    p => new AccountInfoDTO()
                    {
                        Email = p.Email,
                        FirstName = p.FirstName,
                        Id = p.Id,
                        PhoneNumber = p.PhoneNumber,
                        SecondName = p.SecondName
                    })
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<AccountInfoDTO>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
        }

        public async Task<IServiceResult> UpdateAccountAsync(AccountInfoDTO accountInfo)
        {
            try
            {
                var accountId = this.GetAccountId();

                if (accountId != accountInfo.Id)
                {
                    return ServiceResult.GetDefault(
                        false,
                        this.serviceResultMessages.PermissionDenied);
                }

                var accountToUpdate = await this.accountRepository.GetAsync(a => a.Id == accountInfo.Id);

                if (accountToUpdate == null)
                {
                    return ServiceResult.GetDefault(
                        false,
                        this.serviceResultMessages.AccountNotExist);
                }

                accountToUpdate.PhoneNumber = accountInfo.PhoneNumber;
                accountToUpdate.SecondName = accountInfo.SecondName;
                accountToUpdate.FirstName = accountInfo.FirstName;

                await this.accountRepository.UpdateAsync(accountToUpdate);

                await this.accountRepository.SaveAsync();

                return ServiceResult.GetDefault(true);
            }
            catch(Exception ex)
            {
                return ServiceResult.GetDefault(
                    false,
                    ex.Message);
            }
        }

        public async Task<IServiceResult> DeleteAccountAsync()
        {
            try
            {
                var accountId = this.GetAccountId();

                await this.accountRepository.DeleteAsync(a => a.Id == accountId);

                await this.accountRepository.SaveAsync();

                return ServiceResult.GetDefault(true);
            }
            catch(Exception ex)
            {
                return ServiceResult.GetDefault(false, ex.Message);
            }
        }

        public async Task<IServiceResult> CreateAsync(ChangeEntityDTO<ShortAccountInfoDTO> createEntity)
        {
            var isEmailAlreadyExist = await this.accountRepository.AnyAsync(a =>
                a.Email == createEntity.Entity.Email);

            if (isEmailAlreadyExist)
            {
                return this.GetDefaultActionResult(false);
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
                return this.GetDefaultActionResult(false);
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
            throw new NotImplementedException();
            /*
            var isAccountAndCourseExist = await this.ValidateAccountCourseAsync(accountCourseChange);

            if (!isAccountAndCourseExist)
            {
                return this.GetDefaultActionResult(false);
            }

            var isAccountCourseAlreadyExist = await this.accountCourseRepository.AnyAsync(ac =>
                ac.CourseId == accountCourseChange.CourseId
                && ac.AccountId == accountCourseChange.AccountId);

            if (isAccountCourseAlreadyExist)
            {
                return this.GetDefaultActionResult(false);
            }

            await this.accountCourseRepository.CreateAsync(new AccountCourse()
            {
                AccountId = accountCourseChange.AccountId,
                CourseId = accountCourseChange.CourseId,
                Status = CourseStatus.InProgress
            });

            await this.accountCourseRepository.SaveAsync();

            return this.GetDefaultActionResult(true);*/
        }

        public async Task<IServiceResult> RemoveAccountCourseAsync(ChangeAccountCourseDTO accountCourseChange)
        {
            throw new NotImplementedException();
            /*
            var isAccountAndCourseExist = await this.ValidateAccountCourseAsync(accountCourseChange);

            if (!isAccountAndCourseExist)
            {
                return this.GetDefaultActionResult(false);
            }

            await this.accountCourseRepository.DeleteAsync(new AccountCourse()
            {
                AccountId = accountCourseChange.AccountId,
                CourseId = accountCourseChange.CourseId
            });

            await this.accountCourseRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
            */
        }

        public async Task<IServiceResult> ChangeAccountCourseStatusAsync(ChangeAccountCourseDTO accountCourseChange)
        {
            throw new NotImplementedException();
            /*
            var isAccountAndCourseExist = await this.ValidateAccountCourseAsync(accountCourseChange);

            if (!isAccountAndCourseExist)
            {
                return this.GetDefaultActionResult(false);
            }

            var isAccountCourseExist = await this.accountCourseRepository.AnyAsync(ac =>
                (ac.CourseId == accountCourseChange.CourseId && ac.AccountId == accountCourseChange.AccountId)
                && (ac.CourseId == accountCourseChange.CourseId && ac.AccountId == accountCourseChange.AccountId
                && ac.Status != accountCourseChange.Status));

            if (!isAccountCourseExist)
            {
                return this.GetDefaultActionResult(false);
            }

            if (accountCourseChange.Status == CourseStatus.Passed)
            {
                var isAccountPassedAllCourseMaterials = await this.materialService
                    .IsAccountPassedAllCourseMaterials(
                    accountCourseChange.AccountId,
                    accountCourseChange.CourseId);

                if (!isAccountPassedAllCourseMaterials)
                {
                    return this.GetDefaultActionResult(false);
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

            return this.GetDefaultActionResult(true);*/
        }

        public async Task<IServiceResult> AddAccountMaterialAsync(ChangeAccountMaterialDTO accountMaterialChange)
        {
            throw new NotImplementedException();
            /*
            var isAccountAndMaterialExist = await this.ValidateAccountMaterialAsync(accountMaterialChange);

            if (!isAccountAndMaterialExist)
            {
                return this.GetDefaultActionResult(false);
            }

            var isAccountMaterialAlreadyExist = await this.accountMaterialRepository.AnyAsync(am =>
                am.AccountId == accountMaterialChange.AccountId
                && am.MaterialId == accountMaterialChange.MaterialId);

            if (isAccountMaterialAlreadyExist)
            {
                return this.GetDefaultActionResult(false);
            }

            await this.accountMaterialRepository.CreateAsync(new AccountMaterial()
            {
                AccountId = accountMaterialChange.AccountId,
                MaterialId = accountMaterialChange.MaterialId
            });

            await this.accountMaterialRepository.SaveAsync();

            return this.GetDefaultActionResult(true);*/
        }

        public async Task<IServiceResult> RemoveAccountMaterialAsync(ChangeAccountMaterialDTO accountMaterialChange)
        {
            throw new NotImplementedException();
            /*
            var isAccountAndMaterialExist = await this.ValidateAccountMaterialAsync(accountMaterialChange);

            if (!isAccountAndMaterialExist)
            {
                return this.GetDefaultActionResult(false);
            }

            await this.accountMaterialRepository.DeleteAsync(new AccountMaterial()
            {
                AccountId = accountMaterialChange.AccountId,
                MaterialId = accountMaterialChange.MaterialId
            });

            await this.accountMaterialRepository.SaveAsync();

            return this.GetDefaultActionResult(true);*/
        }

        public int GetAccountId()
        {
            var accountIdValue = this.httpContext.HttpContext.User.Claims
                .Where(p => p.Type == ClaimTypes.NameIdentifier)
                .First()
                .Value;

            return int.Parse(accountIdValue);
        }

        /*
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
        */
        /*
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
        */

    }
}
