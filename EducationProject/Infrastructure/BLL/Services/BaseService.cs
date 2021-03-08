using EducationProject.BLL;
using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Services
{
    public class BaseService
    {
        protected IActionResult GetDefaultActionResult(bool actionStatus, string message = null)
        {
            return new ActionResult()
            {
                IsSuccessful = actionStatus,
                MessageCode = message
            };
        }
    }
}
