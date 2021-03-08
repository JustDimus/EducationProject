using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcInterface.ServiceResultController.Interfaces
{
    public interface IServiceResultParser
    {
        string this[string value] { get; }
    }
}
