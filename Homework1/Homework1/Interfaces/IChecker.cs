using Homework1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Homework1.Interfaces
{
    public interface IChecker
    {
        void InitValue(string Number);

        BullXCow Check(string Number);
    }
}
