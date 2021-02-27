using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class ChangeEntityDTO<TEntity> : TokenBasedDTO
    {
        public TEntity Entity { get; set; }
    }
}
