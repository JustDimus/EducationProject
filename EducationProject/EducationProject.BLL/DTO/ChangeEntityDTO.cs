using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class ChangeEntityDTO<TEntity> : AccountIdBasedDTO
    {
        public TEntity Entity { get; set; }
    }
}
