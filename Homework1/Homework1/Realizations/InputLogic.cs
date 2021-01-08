using Homework1.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Homework1.Realizations
{
    public class InputLogic : IInputLogic
    {
        public string InputValue(Predicate<char> Condition)
        {
            string currentValue;
            bool isPassed;

            do
            {
                isPassed = true;
                Console.WriteLine("Enter number: ");

                currentValue = Console.ReadLine();

                foreach (var i in currentValue)
                    if (Condition(i) == false)
                        isPassed = false;
            } while (isPassed == false);

            return currentValue;
        }
    }
}
