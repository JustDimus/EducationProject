using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IConverter<TIn, TOut>
    {
        TOut Get(TIn entity);

        TOut Get(Expression<Func<TIn, bool>> condition);

        IEnumerable<TOut> Get(IEnumerable<TIn> collection);
    }
}
