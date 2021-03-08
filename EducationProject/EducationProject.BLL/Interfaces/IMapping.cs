using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IMapping<TInternal, TExternal>
    {
        TInternal Map(TExternal externalEntity);

        Expression<Func<TInternal, TExternal>> ConvertExpression { get; }
    }
}
