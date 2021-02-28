using EducationProject.BLL.Interfaces;
using EducationProject.Core.DAL.EF;
using Infrastructure.DAL.EF.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EducationProject.BLL.Models;
using CourseStatus = EducationProject.Core.DAL.EF.Enums.ProgressStatus;
using System.Linq.Expressions;
using EducationProject.DAL.Interfaces;

namespace Infrastructure.BLL.Services
{
    public class AccountService : BaseService<AccountDBO, ShortAccountInfoDTO>, IAccountService
    {
        public AccountService(IRepository<AccountDBO> baseEntityRepository, 
            AuthorizationService authorisztionService) 
            : base(baseEntityRepository, authorisztionService)
        {

        }

        protected override Expression<Func<AccountDBO, ShortAccountInfoDTO>> FromBOMapping
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

        protected override Expression<Func<AccountDBO, bool>> IsExistExpression(ShortAccountInfoDTO entity)
        {
            return a => a.Id == entity.Id;
        }

        protected override AccountDBO Map(ShortAccountInfoDTO entity)
        {
            return new AccountDBO()
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

        public override bool Create(ChangeEntityDTO<ShortAccountInfoDTO> createEntity)
        {
            if(ValidateEntity(createEntity.Entity) == false || String.IsNullOrEmpty(createEntity.Entity.Password))
            {
                return false;
            }

            if(entity.Any(a => a.Email == createEntity.Entity.Email) == true)
            {
                return false;
            }

            entity.Create(Map(createEntity.Entity));

            entity.Save();

            return true;
        }
    }
}
