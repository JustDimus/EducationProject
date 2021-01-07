using Homework1.Interfaces;
using Homework1.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Homework1.Realizations
{
    public class GameLogic : IGameLogic
    {
        private int _maxValue = 9;

        private int _valueLength;

        private List<string> _combinations;

        private string _lastValue;

        public GameLogic()
        {
            _combinations = new List<string>();
        }

        private string StartValue()
        {
            Random rand = new Random();
            char fVal = (char)rand.Next('0', ('9' + 1));
            char sVal;
            do
            {
                sVal = (char)rand.Next('0', ('9' + 1));
            } while (sVal == fVal);

            string result = "";
            result += fVal;
            result += fVal;
            result += sVal;
            result += sVal;

            return "1234";
        }

        private void Combination(string already)
        {
            for (char f = '0'; f <= '9'; f++)
            {
                if (already.Length + 1 == _valueLength)
                {
                    if (already.Contains(f) == false)
                        _combinations.Add(already + f);
                }
                else
                {
                    if (already.Contains(f) == false)
                        Combination(already + f);
                }
            }
        }

        private void GenerateCombinations()
        {
            Combination("");
        }

        private void ClearCombinations(BullXCow PrevResult)
        {
            if (PrevResult.Bulls == 0 && PrevResult.Cows == 0)
            {
                foreach (var i in _lastValue)
                    _combinations.RemoveAll(s => s.Contains(i));
            }
            else
            {
                _combinations.RemoveAll(s => CheckCombination(s, _lastValue, PrevResult));
            }
        }

        private bool CheckCombination(string Element, string Value, BullXCow PrevResult)
        {
            int overlap = 0;
            for (int i = 0; i < _valueLength; i++)
            {
                if (Value[i] == Element[i])
                {
                    overlap++;
                }
            }

            if (overlap != PrevResult.Bulls)
            {
                if (Element == "3486")
                    if (true)
                    {

                    }
                return false;
            }

            overlap = 0;

            for (int i = 0; i < _valueLength; i++)
                if (Element.Contains(Value[i]) && Value[i] != Element[i])
                {
                    overlap++;
                }

            if (overlap < PrevResult.Cows)
            {
                if (Element == "3486")
                    if (true)
                    {

                    }
                return false;
            }
            return true;
        }

        private string GetLikelyCombination()
        {
            return _combinations[new Random().Next(_combinations.Count)];
            /*
            string result = "";
            int count = _combinations.Count + 1;

            foreach (var i in _combinations)
            {
                int c = count;
                foreach (var t in _combinations)
                {

                }
            }

            if(result == "")
                if(true)
                {

                }
            return result;*/
        }

        public string Run(BullXCow PrevResult)
        {
            if (PrevResult == null)
            {
                GenerateCombinations();

                _lastValue = StartValue();

                return _lastValue;
            }

            ClearCombinations(PrevResult);

            _lastValue = GetLikelyCombination();

            return _lastValue;
        }

        public void SetLength(int Value)
        {
            _valueLength = Value;
        }
    }
}
