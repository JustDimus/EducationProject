using Homework1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Homework1.Interfaces
{
    public interface ILogger
    {
        void Log(string Message);

        void Log(BullXCow Result, string Value);

        void Wait();
    }
}
