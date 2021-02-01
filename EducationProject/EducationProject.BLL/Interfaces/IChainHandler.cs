using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IChainHandler
    {
        IChain this[string Command]
        {
            get;
        }
    }
}
