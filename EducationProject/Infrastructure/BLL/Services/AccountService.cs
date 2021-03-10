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
using EducationProject.Core.Models.Enums;

namespace EducationProject.Infrastructure.BLL.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private IRepository<AccountCourse> accountCourseRepository;

        private IRepository<AccountMaterial> accountMaterialRepository;

        private IRepository<Account> accountRepository;

        private IMaterialService materialService;

        private ISkillService skillService;

        private ICourseService courseService;

        private AccountMapping accountMapping;

        private ServiceResultMessageCollection serviceResultMessages;

        private IPasswordHasher passwordHasher;

        private IAuthorizationService authorizationService;

        private int defaultAccountInfoPageSize;

        public AccountService(
            IRepository<Account> accountRepository,
            IRepository<AccountCourse> accountCoursesRepository,
            IRepository<AccountMaterial> accountMaterialsRepository,
            IMaterialService materialService,
            ISkillService skillService,
            ICourseService courseService,
            AccountMapping accountMapping,
            ServiceResultMessageCollection serviceResultMessageCollection,
            IAuthorizationService authorizationService,
            IPasswordHasher passwordHasher)
        {
            this.accountCourseRepository = accountCoursesRepository;
            this.accountMaterialRepository = accountMaterialsRepository;
            this.accountRepository = accountRepository;

            this.authorizationService = authorizationService;

            this.skillService = skillService;
            this.materialService = materialService;
            this.courseService = courseService;

            this.accountMapping = accountMapping;

            this.serviceResultMessages = serviceResultMessageCollection;

            this.passwordHasher = passwordHasher;

            this.defaultAccountInfoPageSize = 10;
        }

        public async Task<IServiceResult<FullAccountInfoDTO>> GetAccountFullInfoAsync(
            PageInfoDTO coursePageInfo, 
            PageInfoDTO skillPageInfo)
        {
            try
            {
                var result = await this.accountRepository.GetAsync<FullAccountInfoDTO>(
                    a => a.Id == this.authorizationService.GetAccountId(),
                    this.accountMapping.FullInfoConvertExpression);

                result.PassedCoursesCount = await this.accountCourseRepository.CountAsync(
                    ac => ac.AccountId == this.authorizationService.GetAccountId() 
                    && ac.Status == CourseStatus.Passed);

                var accountCoursesServiceResult = await this.courseService.GetAccountCourseProgressPageAsync(coursePageInfo);

                if(!accountCoursesServiceResult.IsSuccessful)
                {
                    return new ServiceResult<FullAccountInfoDTO>()
                    {
                        IsSuccessful = false,
                        MessageCode = accountCoursesServiceResult.MessageCode
                    };
                }

                var accountSkillsServiceResult = await this.skillService.GetAccountSkillProgressPageAsync(skillPageInfo);

                if(!accountSkillsServiceResult.IsSuccessful)
                {
                    return new ServiceResult<FullAccountInfoDTO>()
                    {
                        IsSuccessful = false,
                        MessageCode = accountSkillsServiceResult.MessageCode
                    };
                }

                result.Skills = accountSkillsServiceResult.Result;

                return new ServiceResult<FullAccountInfoDTO>()
                {
                    IsSuccessful = true,
                    Result = result
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<FullAccountInfoDTO>()
                {
                    IsSuccessful = false,
                    MessageCode = ex.Message
                };
            }
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
            var accountId = this.authorizationService.GetAccountId();

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
                var accountId = this.authorizationService.GetAccountId();

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
                var accountId = this.authorizationService.GetAccountId();

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
            try
            {
                var isAccountAndCourseExist = await this.ValidateAccountCourseAsync(accountCourseChange);

                if (!isAccountAndCourseExist)
                {
                    return ServiceResult.GetDefault(false);
                }

                var isAccountCourseAlreadyExist = await this.accountCourseRepository.AnyAsync(ac =>
                    ac.CourseId == accountCourseChange.CourseId
                    && ac.AccountId == this.authorizationService.GetAccountId());

                if (isAccountCourseAlreadyExist)
                {
                    return ServiceResult.GetDefault(false);
                }

                await this.accountCourseRepository.CreateAsync(new AccountCourse()
                {
                    AccountId = this.authorizationService.GetAccountId(),
                    CourseId = accountCourseChange.CourseId,
                    Status = CourseStatus.InProgress
                });

                await this.accountCourseRepository.SaveAsync();

                return ServiceResult.GetDefault(true);
            }
            catch(Exception ex)
            {
                return ServiceResult.GetDefault(
                    true,
                    ex.Message);
            }
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
            var isAccountAndCourseExist = await this.ValidateAccountCourseAsync(accountCourseChange);

            if (!isAccountAndCourseExist)
            {
                return ServiceResult.GetDefault(
                    false,
                    this.serviceResultMessages.AccountOrCourseNotExist);
            }

            var isAccountCourseExist = await this.accountCourseRepository.AnyAsync(ac =>
                (ac.CourseId == accountCourseChange.CourseId && ac.AccountId == this.authorizationService.GetAccountId())
                && (ac.CourseId == accountCourseChange.CourseId && ac.AccountId == this.authorizationService.GetAccountId()
                && ac.Status != accountCourseChange.Status));

            if (!isAccountCourseExist)
            {
                return this.GetDefaultActionResult(
                    false,
                    this.serviceResultMessages.AccountCourseNotExist);
            }

            if (accountCourseChange.Status == CourseStatus.Passed)
            {
                var isAccountPassedAllCourseMaterials = await this.materialService.IsAccountPassedAllCourseMaterialsAsync(
                    this.authorizationService.GetAccountId(),
                    accountCourseChange.CourseId);

                if (!isAccountPassedAllCourseMaterials)
                {
                    return this.GetDefaultActionResult(
                        false,
                        this.serviceResultMessages.AccountDidNotPassCourseMaterials);
                }
            }

            var accountCourse = await this.accountCourseRepository.GetAsync(ac =>
                ac.AccountId == this.authorizationService.GetAccountId()
                && ac.CourseId == accountCourseChange.CourseId);

            if (!accountCourse.OncePassed && accountCourseChange.Status == CourseStatus.Passed)
            {
                accountCourse.OncePassed = true;

                await this.skillService.AddSkilsToAccountByCourseIdAsync(new AddSkillsToAccountByCourseDTO()
                {
                    AccountId = this.authorizationService.GetAccountId(),
                    CourseId = accountCourseChange.CourseId
                });
            }

            accountCourse.Status = accountCourseChange.Status.Value;

            await this.accountCourseRepository.UpdateAsync(accountCourse);

            await this.accountCourseRepository.SaveAsync();

            return this.GetDefaultActionResult(true);
        }

        public async Task<IServiceResult> AddAccountMaterialAsync(int materialId)
        {
            var isAccountAndMaterialExist = await this.ValidateAccountMaterialAsync(materialId);

            if (!isAccountAndMaterialExist)
            {
                return ServiceResult.GetDefault(
                    false,
                    this.serviceResultMessages.AccountOrMaterialNotExist);
            }

            var isAccountMaterialAlreadyExist = await this.accountMaterialRepository.AnyAsync(am =>
                am.AccountId == this.authorizationService.GetAccountId()
                && am.MaterialId == materialId);

            if (isAccountMaterialAlreadyExist)
            {
                return ServiceResult.GetDefault(
                    false,
                    this.serviceResultMessages.AccountMaterialExist);
            }

            await this.accountMaterialRepository.CreateAsync(new AccountMaterial()
            {
                AccountId = this.authorizationService.GetAccountId(),
                MaterialId = materialId
            });

            await this.accountMaterialRepository.SaveAsync();

            return ServiceResult.GetDefault(true);
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
        
        private async Task<bool> ValidateAccountMaterialAsync(int materialId)
        {
            var isMaterialExist = await this.materialService.IsExistAsync(materialId);

            var isAccountExist = await this.accountRepository.AnyAsync(a =>
                a.Id == this.authorizationService.GetAccountId());

            if (!isMaterialExist || !isAccountExist)
            {
                return false;
            }

            return true;
        }
        
        private async Task<bool> ValidateAccountCourseAsync(ChangeAccountCourseDTO changeAccountCourse)
        {
            var isCourseExist = await this.courseService.IsExistAsync(changeAccountCourse.CourseId);

            var isAccountExist = await this.accountRepository.AnyAsync(a =>
                a.Id == this.authorizationService.GetAccountId());

            if (!isCourseExist || !isAccountExist)
            {
                return false;
            }

            return true;
        }
    }
}
