using Homework1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Homework1.Interfaces
{
    public interface IGameLogic
    {
        string Run(BullXCow PrevResult);

        void SetLength(int Value);
    }
}
