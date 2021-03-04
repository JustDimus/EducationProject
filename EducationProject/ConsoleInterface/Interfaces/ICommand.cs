using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInterface.Interfaces
{
    public interface ICommand
    {
        string Name { get; }

        Task Run(int accountId);
    }
}
