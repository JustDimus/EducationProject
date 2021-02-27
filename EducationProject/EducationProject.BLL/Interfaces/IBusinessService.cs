using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IBusinessService<TEntity>
    {
        bool Create(ChangeEntityDTO<TEntity> createEntity);

        bool Update(ChangeEntityDTO<TEntity> updateEntity);

        bool Delete(ChangeEntityDTO<TEntity> deleteEntity);

        IEnumerable<TEntity> Get(PageInfoDTO pageInfo);

        TEntity GetInfo(TEntity entity);
    }
}
