using Homework1.Interfaces;
using Homework1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Homework1.Realizations
{
    public class GameController : IController
    {
        IChecker _checker;

        IGameLogic _gameLogic;

        ILogger _logger;

        IInputLogic _inputLogic;

        public GameController(IChecker Checker, IGameLogic GameLogic, ILogger Logger, IInputLogic InputLogic)
        {
            _checker = Checker;
            _gameLogic = GameLogic;
            _logger = Logger;
            _inputLogic = InputLogic;
        }

        public void Start()
        {
            int currentStep = 1;
            BullXCow currentResult = null;
            string currentValue;

            _logger.Log("Game starts!");

            currentValue = _inputLogic.InputValue(c => c >= '0' && c <= '9');

            _logger.Log($"You've entered value: {currentValue}");

            _checker.InitValue(currentValue);
            _gameLogic.SetLength(currentValue.Length);

            do
            {
                _logger.Log($"Step: {currentStep}");

                currentValue = _gameLogic.Run(currentResult);

                currentResult = _checker.Check(currentValue);

                _logger.Log(currentResult, currentValue);

                currentStep += 1;
            } while (currentResult.Bulls != 4);

            _logger.Log($"You win!\nValue = {currentValue}\nRequired steps = {currentStep - 1}");
            _logger.Wait();
        }
    }
}
