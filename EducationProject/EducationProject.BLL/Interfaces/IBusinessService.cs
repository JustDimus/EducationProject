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
        Task<IActionResult> CreateAsync(ChangeEntityDTO<TEntity> createEntity);

        Task<IActionResult> UpdateAsync(ChangeEntityDTO<TEntity> updateEntity);

        Task<IActionResult> DeleteAsync(ChangeEntityDTO<TEntity> deleteEntity);

        Task<IActionResult<bool>> IsExistAsync(TEntity checkEntity);

        Task<IActionResult<IEnumerable<TEntity>>> GetAsync(PageInfoDTO pageInfo);
    }
}
