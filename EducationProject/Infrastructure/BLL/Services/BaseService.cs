using EducationProject.BLL;
using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Infrastructure.BLL.Services
{
    public class BaseService
    {
        protected IServiceResult GetDefaultActionResult(bool actionStatus, string message = null)
        {
            return new ServiceResult()
            {
                IsSuccessful = actionStatus,
                MessageCode = message
            };
        }
    }
}
