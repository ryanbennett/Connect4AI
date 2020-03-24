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

            Console.WriteLine("(P)lay or (L)oad game? ");
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
            else
            {
                PlayGame(new Game());

            }
        }

        private static void PlayGame(Game game)
        {
            
            var result = game.Play();
            while (result)
            {
                result = game.Play();
            }

            game.log.Save();
        }
    }
}
