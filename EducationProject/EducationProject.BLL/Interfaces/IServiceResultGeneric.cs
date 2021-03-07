using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IServiceResult<TEntity> : IServiceResult
    {
        TEntity Result { get; set; }
    }
}
