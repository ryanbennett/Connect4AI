using System;
using Connect4AI.Core;

namespace Connect4AI.WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
           
                var game = new WebJobGame(new GameLogAzureStorage());

                while (game.PlayAutomated(false)) { };
                game.log.Save();
          
            }
        }
    }
}
