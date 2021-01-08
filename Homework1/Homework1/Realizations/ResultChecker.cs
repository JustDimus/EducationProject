using Homework1.Interfaces;
using Homework1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Homework1.Realizations
{
    public class ResultChecker : IChecker
    {
        private string _resultValue;

        public BullXCow Check(string Number)
        {
            if (Number.Length != _resultValue.Length)
                throw new ArgumentException("Incorrect value lenght");
            
            int C = 0; //Total cows
            int B = 0; //Total bulls 

            for(int i = 0; i < _resultValue.Length; i++)
            {
                if (_resultValue[i] == Number[i])
                {
                    B++;
                }
                else
                {
                    if(Number.Contains(_resultValue[i]) == true)
                    {
                        C++;
                    }
                }
            }

            return new BullXCow(C, B);
        }

        public void InitValue(string Number)
        {
            _resultValue = Number;
        }
    }
}
