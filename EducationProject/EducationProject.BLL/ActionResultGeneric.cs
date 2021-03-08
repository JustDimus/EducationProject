using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL
{
    public class ActionResult<TEntity> : ActionResult, IActionResult<TEntity>
    {        
        public TEntity Result { get; set; }
    }
}
