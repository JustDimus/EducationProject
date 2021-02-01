using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface ICommandHandler
    {
        ICommand this[string Command]
        {
            get;
        }
    }
}
