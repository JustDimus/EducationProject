using EducationProject.BLL.DTO;
using EducationProject.BLL.Interfaces;
using EducationProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EducationProject.Infrastructure.BLL.Mappings
{
    public class AccountMapping : IMapping<Account, ShortAccountInfoDTO>
    {
        public Expression<Func<Account, ShortAccountInfoDTO>> ConvertExpression
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

        public Account Map(ShortAccountInfoDTO externalEntity)
        {
            return new Account()
            {
                Email = externalEntity.Email,
                RegistrationDate = externalEntity.RegistrationDate,
                FirstName = externalEntity.FirstName,
                Id = externalEntity.Id,
                Password = externalEntity.Password,
                PhoneNumber = externalEntity.PhoneNumber,
                SecondName = externalEntity.SecondName
            };
        }
    }
}
