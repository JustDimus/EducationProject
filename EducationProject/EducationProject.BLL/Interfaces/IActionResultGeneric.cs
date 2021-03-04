using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IActionResult<TEntity> : IActionResult
    {
        TEntity Result { get; set; }
    }
}
