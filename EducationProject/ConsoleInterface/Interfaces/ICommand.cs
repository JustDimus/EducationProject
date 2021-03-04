using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Interfaces
{
    public interface ICommand
    {
        string Name { get; }

        void Run(int accountId);
    }
}
