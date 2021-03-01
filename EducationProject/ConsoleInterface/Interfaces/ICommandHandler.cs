using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Interfaces
{
    public interface ICommandHandler
    {
        ICommand this[string value]
        {
            get;
        }
    }
}
