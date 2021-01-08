using Homework1.Models;
using Homework1.Realizations;
using System;

namespace Homework1
{
    class Program
    {
        static GameController Game;

        static void Register()
        {
            Game = new GameController(
                new ResultChecker(),
                new GameLogic(),
                new ConsoleLogger(),
                new InputLogic());
        }

        static void Main(string[] args)
        {
            Register();
            Game.Start();
        }
    }
}
