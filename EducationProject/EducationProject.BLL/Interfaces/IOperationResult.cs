using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public enum ResultType { Failed, Success }

    public interface IOperationResult
    {
        ResultType Status { get; set; }

        object Result { get; set; }
    }
}
