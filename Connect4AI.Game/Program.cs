using System;

namespace Connect4AI.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            var result = game.Play();
            while (result)
            {
                result = game.Play();
            }

            game.log.Save();
            
        }
    }
}
