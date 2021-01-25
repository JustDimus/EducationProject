using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.PL
{

    public class OperationResult: IOperationResult
    {
        public ResultType Status { get; set; }
        
        public object Result { get; set; }     
    }
}
