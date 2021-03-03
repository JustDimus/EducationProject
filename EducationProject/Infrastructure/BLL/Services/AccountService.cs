using EducationProject.BLL.Interfaces;
using EducationProject.Core.Models;
using Infrastructure.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EducationProject.BLL.DTO;
using CourseStatus = EducationProject.Core.Models.Enums.ProgressStatus;
using System.Linq.Expressions;
using EducationProject.DAL.Interfaces;

namespace Infrastructure.BLL.Services
{
    public class AccountService : BaseService<Account, ShortAccountInfoDTO>, IAccountService
    {
        private IRepository<AccountCourse> accountCourses { get; set; }

        private IRepository<AccountMaterial> accountMaterials { get; set; }

        private ICourseService courses;

        private IMaterialService materials;

        private ISkillService skills;

        public AccountService(IRepository<Account> baseEntityRepository, 
            AuthorizationService authorisztionService,
            IRepository<AccountCourse> accountCoursesRepository,
            IRepository<AccountMaterial> accountMaterialsRepository,
            ICourseService courseService,
            IMaterialService materialService,
            ISkillService skillService) 
            : base(baseEntityRepository, authorisztionService)
        {
            this.accountCourses = accountCoursesRepository;
            this.accountMaterials = accountMaterialsRepository;

            this.courses = courseService;
            this.skills = skillService;
            this.materials = materialService;

        }

        protected override Expression<Func<Account, ShortAccountInfoDTO>> FromBOMapping
        {
            get => a => new ShortAccountInfoDTO()
            {
                Email = a.Email,
                RegistrationDate = a.RegistrationDate,
                FirstName = a.FirstName,
                Password = a.Password,
                PhoneNumber = a.PhoneNumber,
                SecondName = a.SecondName
            };
        }

        protected override Expression<Func<Account, bool>> IsExistExpression(ShortAccountInfoDTO entity)
        {
            return a => a.Id == entity.Id;
        }

        public bool AddAccountCourse(ChangeAccountCourseDTO accountCourseChange)
        {
            int accountId = this.Authenticate(accountCourseChange.Token);

            if(accountId == 0)
            {
                return false;
            }

            accountCourseChange.AccountId = accountId;

            if(ValidateAccountCourse(accountCourseChange) == false)
            {
                return false;
            }

            if(accountCourses.Any(ac => ac.AccountId == accountCourseChange.AccountId 
            && ac.CourseId == accountCourseChange.CourseId) == true)
            {
                return false;
            }

            this.accountCourses.Create(new AccountCourse()
            {
                AccountId = accountCourseChange.AccountId,
                CourseId = accountCourseChange.CourseId,
                Status = CourseStatus.InProgress
            });

            this.accountCourses.Save();

            return true;
        }

        public bool RemoveAccountCourse(ChangeAccountCourseDTO accountCourseChange)
        {
            int accountId = this.Authenticate(accountCourseChange.Token);

            if (accountId == 0)
            {
                return false;
            }

            accountCourseChange.AccountId = accountId;

            if (ValidateAccountCourse(accountCourseChange) == false)
            {
                return false;
            }

            this.accountCourses.Delete(ac => ac.AccountId == accountCourseChange.AccountId
            && ac.CourseId == accountCourseChange.CourseId);

            this.accountCourses.Save();

            return true;
        }

        public bool ChangeAccountCourseStatus(ChangeAccountCourseDTO accountCourseChange)
        {
            int accountId = this.Authenticate(accountCourseChange.Token);

            if (accountId == 0 || accountCourseChange.Status is null)
            {
                return false;
            }

            accountCourseChange.AccountId = accountId;

            if (accountCourses.Any(ac => ac.AccountId == accountCourseChange.AccountId
             && ac.CourseId == accountCourseChange.CourseId) == false)
            {
                return false;
            }

            if (accountCourses.Any(ac => ac.AccountId == accountCourseChange.AccountId
             && ac.CourseId == accountCourseChange.CourseId && ac.Status == accountCourseChange.Status.Value) == true)
            {
                return false;
            }

            if (accountCourseChange.Status == CourseStatus.Passed)
            {
                var idList = this.courses.GetAllCourseMaterialsId(accountCourseChange.CourseId).ToList();
                foreach (var materialId in idList)
                {
                    if(this.accountMaterials.Any(am => am.AccountId == accountCourseChange.AccountId
                    && am.MaterialId == materialId) == false)
                    {
                        return false;
                    }
                }
            }

            var accountCourse = this.accountCourses.Get(ac => ac.AccountId == accountCourseChange.AccountId
            && ac.CourseId == accountCourseChange.CourseId);

            if(accountCourse.OncePassed == false && accountCourseChange.Status == CourseStatus.Passed)
            {
                accountCourse.OncePassed = true;

                skills.AddSkilsToAccountByCourseId(new AddSkillsToAccountByCourseDTO()
                {
                    AccountId = accountCourseChange.AccountId,
                    CourseId = accountCourseChange.CourseId
                });
            }

            accountCourse.Status = accountCourseChange.Status.Value;

            this.accountCourses.Update(accountCourse);

            this.accountCourses.Save();

            return true;
        }

        public bool AddAccountMaterial(ChangeAccountMaterialDTO accountMaterialChange)
        {
            int accountId = this.Authenticate(accountMaterialChange.Token);

            if (accountId == 0)
            {
                return false;
            }

            accountMaterialChange.AccountId = accountId;

            if (ValidateAccountMaterial(accountMaterialChange) == false)
            {
                return false;
            }

            if(this.accountMaterials.Any(am => am.AccountId == accountMaterialChange.AccountId
            && am.MaterialId == accountMaterialChange.MaterialId) == true)
            {
                return false;
            }

            this.accountMaterials.Create(new AccountMaterial()
            {
                AccountId = accountMaterialChange.AccountId,
                MaterialId = accountMaterialChange.MaterialId
            });

            this.accountMaterials.Save();

            return true;
        }

        public bool RemoveAccountMaterial(ChangeAccountMaterialDTO accountMaterialChange)
        {
            int accountId = this.Authenticate(accountMaterialChange.Token);

            if (accountId == 0 || accountId != accountMaterialChange.AccountId)
            {
                return false;
            }


            if (ValidateAccountMaterial(accountMaterialChange) == false)
            {
                return false;
            }

            this.accountMaterials.Delete(am => am.AccountId == accountMaterialChange.AccountId
            && am.MaterialId == accountMaterialChange.MaterialId);

            this.accountCourses.Save();

            return true;
        }

        public override bool Create(ChangeEntityDTO<ShortAccountInfoDTO> createEntity)
        {
            if (ValidateEntity(createEntity.Entity) == false || String.IsNullOrEmpty(createEntity.Entity.Password))
            {
                return false;
            }

            if (entity.Any(a => a.Email == createEntity.Entity.Email) == true)
            {
                return false;
            }

            var account = Map(createEntity.Entity);

            account.RegistrationDate = DateTime.Now;

            entity.Create(account);

            entity.Save();

            return true;
        }

        public FullAccountInfoDTO GetAccountInfo(int id)
        {
            var result = entity.Get<FullAccountInfoDTO>(a => a.Id == id, a => new FullAccountInfoDTO()
            {
                Id = a.Id,
                RegistrationDate = a.RegistrationDate,
                Email = a.Email,
                Password = null,
                SecondName = a.SecondName,
                FirstName = a.FirstName,
                PhoneNumber = a.PhoneNumber
            });

            result.PassedCoursesCount = accountCourses.Count(ac => ac.AccountId == id
            && ac.Status == CourseStatus.Passed);

            result.PassedCourses = accountCourses.GetPage<AccountCourseDTO>(ac => ac.AccountId == id
            && ac.Status == CourseStatus.Passed, ac => new AccountCourseDTO()
            {
                Title = ac.Course.Title,
                CourseId = ac.CourseId,
                Status = CourseStatus.Passed
            }, 0, defaultGetCount);

            result.CoursesInProgressCount = accountCourses.Count(ac => ac.AccountId == id
            && ac.Status == CourseStatus.InProgress);

            result.CoursesInProgress = accountCourses.GetPage<AccountCourseDTO>(ac => ac.AccountId == id
            && ac.Status == CourseStatus.InProgress, ac => new AccountCourseDTO()
            {
                Title = ac.Course.Title,
                CourseId = ac.CourseId,
                Status = CourseStatus.InProgress
            }, 0, defaultGetCount);

            result.AccountSkills = skills.GetAccountSkills(new GetAccountSkillsDTO()
            {
                AccountId = id,
                PageNumber = 0,
                PageSize = defaultGetCount
            });

            return result;
        }

        public FullAccountInfoDTO GetAccountInfo(string token)
        {
            int accountId = Authenticate(token);

            if(accountId == 0)
            {
                return null;
            }
            else
            {
                return this.GetAccountInfo(accountId);
            }
        }

        protected override Account Map(ShortAccountInfoDTO entity)
        {
            return new Account()
            {
                Email = entity.Email,
                RegistrationDate = entity.RegistrationDate,
                FirstName = entity.FirstName,
                Id = entity.Id,
                Password = entity.Password,
                PhoneNumber = entity.PhoneNumber,
                SecondName = entity.SecondName
            };
        }

        protected override bool ValidateEntity(ShortAccountInfoDTO entity)
        {
            if(String.IsNullOrEmpty(entity.Email))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateAccountMaterial(ChangeAccountMaterialDTO changeAccountMaterial)
        {
            if (changeAccountMaterial.MaterialId == 0
                || changeAccountMaterial.AccountId == 0)
            {
                return false;
            }

            if (this.IsExist(a => a.Id == changeAccountMaterial.AccountId) == false
                || materials.IsExist(new MaterialDTO() { Id = changeAccountMaterial.MaterialId }) == false)
            {
                return false;
            }

            return true;
        }

        private bool ValidateAccountCourse(ChangeAccountCourseDTO changeAccountCourse)
        {
            if(changeAccountCourse.CourseId == 0
                || changeAccountCourse.AccountId == 0)
            {
                return false;
            }

            if(this.IsExist(a => a.Id == changeAccountCourse.AccountId) == false
                || courses.IsExist(new ShortCourseInfoDTO() { Id = changeAccountCourse.CourseId }) == false)
            {
                return false;
            }

            return true;
        }
    }
}
