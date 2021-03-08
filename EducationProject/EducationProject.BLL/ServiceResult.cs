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

        public static IServiceResult GetDefault(bool isSuccessful, string messageCode = null)
        {
            return new ServiceResult()
            {
                IsSuccessful = isSuccessful,
                MessageCode = messageCode
            };
        }
    }
}
