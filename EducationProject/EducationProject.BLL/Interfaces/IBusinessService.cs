using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IBusinessService<TEntity>
    {
        bool Create(ChangeEntityDTO<TEntity> createEntity);

        bool Update(ChangeEntityDTO<TEntity> updateEntity);

        bool Delete(ChangeEntityDTO<TEntity> deleteEntity);

        bool IsExist(TEntity checkEntity);

        IEnumerable<TEntity> Get(PageInfoDTO pageInfo);

        string LogIn(AccountAuthorizationDataDTO authData);

        bool LogOut(string token);
    }
}
