using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Interfaces
{

    public interface ISectionHandler
    {
        object ResultData { get; }

        void Run(AccountAuthenticationData data = null);
    }
}
