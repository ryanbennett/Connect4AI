using System;
using System.IO;
using System.Windows.Forms;

namespace Connect4AI.Game
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            Console.WriteLine("(A)utomate, (P)lay or (L)oad game? ");
            var key = Console.ReadKey();
            var filePath = string.Empty;

            if(key.Key == ConsoleKey.L)
            {
                OpenFileDialog fbd = new OpenFileDialog();
                fbd.InitialDirectory = @"C:\Users\rbenn\source\repos\Connect4AI\GameLogs";


                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    filePath = fbd.FileName;

                    var game = new Game();
                    var startNewGame = game.Replay(filePath);

                    if (startNewGame)
                    {
                        PlayGame(game);
                    }

                }
            }
            else if(key.Key == ConsoleKey.P)
            {

                Console.WriteLine();
                Console.WriteLine("(1) or (2) players?");
                key = Console.ReadKey();

                var twoPlayer = key.Key == ConsoleKey.D1 ? false : true;

                PlayGame(new Game(),twoPlayer);

            }

            else if(key.Key == ConsoleKey.A)
            {
                Console.WriteLine();
                Console.WriteLine("How many games? ");
                var games = int.Parse(Console.ReadLine());
                PlayAutoGame(games);
            }
        }

        private static void PlayAutoGame(int games)
        {
            for(var i=0; i<games; i++)
            {
                var game = new Game();

                while (game.PlayAutomated(false)) { };
                game.log.Save();

            }
        }

        private static void PlayGame(Game game, bool twoPlayer = false)
        {
            
            var result = game.Play(twoPlayer);
            while (result)
            {
                result = game.Play(twoPlayer);
            }

            game.log.Save();
        }


    }
}
