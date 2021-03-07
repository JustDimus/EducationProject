using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EducationProject.BLL.Interfaces
{
    public interface IBusinessService<TEntity>
    {
        Task<IServiceResult> CreateAsync(ChangeEntityDTO<TEntity> createEntity);

        Task<IServiceResult> UpdateAsync(ChangeEntityDTO<TEntity> updateEntity);

        Task<IServiceResult> DeleteAsync(ChangeEntityDTO<TEntity> deleteEntity);

        Task<IServiceResult<bool>> IsExistAsync(TEntity checkEntity);

        Task<IServiceResult<IEnumerable<TEntity>>> GetAsync(PageInfoDTO pageInfo);
    }
}
