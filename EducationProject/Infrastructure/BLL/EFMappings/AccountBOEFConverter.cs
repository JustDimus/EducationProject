using EducationProject.BLL.Interfaces;
using EducationProject.Core.DAL.EF;
using EducationProject.Core.PL.EF;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.BLL.EFMappings
{
    public class AccountBOEFConverter : IConverter<AccountDBO, AccountPL>
    {
        private IConverter<CourseDBO, CoursePL> courses;

        private IConverter<SkillDBO, SkillPL> skills;

        private IMapping<AccountDBO> accountMapping;

        public AccountBOEFConverter(IMapping<AccountDBO> accountMapping, IConverter<CourseDBO, CoursePL> courseConverter, 
            IConverter<SkillDBO, SkillPL> skillConverter)
        {
            courses = courseConverter;

            skills = skillConverter;

            this.accountMapping = accountMapping;
        }

        public AccountPL Get(AccountDBO entity)
        {
            throw new NotImplementedException();
        }

        public AccountPL Get(Expression<Func<AccountDBO, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AccountPL> Get(IEnumerable<AccountDBO> collection)
        {
            throw new NotImplementedException();
        }
    }
}
