using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IMapper<TIn, TOut>
    {
        TOut Get(TIn entity);

        IEnumerable<TOut> Get(Expression<Func<TIn, bool>> condition, int pageNumber = 0, int pageSize = 30);

        IEnumerable<TOut> Get(IEnumerable<TIn> collection);
    }
}
