using System;
using System.Collections.Generic;
using System.Text;

namespace Homework1.Models
{
    public class BullXCow
    {
        public int Cows { get; }

        public int Bulls { get; }

        public BullXCow(int Cow, int Bull)
        {
            Cows = Cow;
            Bulls = Bull;
        }
    }
}
