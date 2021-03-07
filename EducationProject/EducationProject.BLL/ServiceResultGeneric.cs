using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL
{
    public class ServiceResult<TEntity> : ServiceResult, IServiceResult<TEntity>
    {        
        public TEntity Result { get; set; }
    }
}
