using System;
using System.Collections.Generic;
using System.Text;

namespace Homework1.Interfaces
{
    public interface IInputLogic
    {
        string InputValue(Predicate<char> Condition);
    }
}
