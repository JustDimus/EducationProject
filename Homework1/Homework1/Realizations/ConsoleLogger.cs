using Homework1.Interfaces;
using Homework1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Homework1.Realizations
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string Message)
        {
            Console.WriteLine(Message);
        }

        public void Log(BullXCow Result, string Value)
        {
            this.Log($"V: {Value}, B: {Result.Bulls}, C: {Result.Cows}");
        }

        public void Wait()
        {
            Console.ReadKey();
        }
    }
}
