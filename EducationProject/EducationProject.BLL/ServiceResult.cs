using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL
{
    public class ServiceResult : IServiceResult
    {
        public bool IsSuccessful { get; set; }

        public string MessageCode { get; set; }
    }
}
