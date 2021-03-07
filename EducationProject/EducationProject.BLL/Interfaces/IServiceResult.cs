using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IServiceResult
    {
        public bool IsSuccessful { get; set; }

        public string MessageCode { get; set; }
    }
}
