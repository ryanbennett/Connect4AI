using Connect4AI.Game;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace GamesToCSV
{
    public class Class1
    {

       public void Run()
        {
            var files = new DirectoryInfo(@"C:\Users\rbenn\source\repos\Connect4AI\GameLogs\FinishedGames").GetFiles();

            var csv = File.CreateText(@"C:\Users\rbenn\source\repos\Connect4AI\GameLogs\Games.csv");
            csv.Dispose();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("i0\ti1\ti2\ti3\ti4\ti5\ti6\ti7\ti8\ti9\ti10\ti11\ti12\ti13\ti14\ti15\ti16\ti17\ti18\ti19\ti20\ti21\ti22\ti23\ti24\ti25\ti26\ti27\ti28\ti29\ti30\ti31\ti32\ti33\ti34\ti35\ti36\ti37\ti38\ti39\ti40\ti41\tindexPlayed");
            foreach(var file in files)
            {
                var text = File.ReadAllText(file.FullName);
                GameLog gameLog;
                try
                {
                   gameLog = JsonConvert.DeserializeObject<GameLog>(text);
                }
                catch
                {
                    continue;
                }
                

              
                foreach(var entry in gameLog.Log.Where(e => e.PlayerNumber == 2))
                {
                    var csvRow = string.Empty;
                    for (var r=0; r< 6; r++)
                    {
                        for(var c = 0; c < 7; c++)
                        {
                            var value = entry.BoardBeforeMove[r, c];
                            if(value == 2)
                            {
                                value = -1;
                            }

                            csvRow += value + "\t";
                        }
                    }

                    csvRow += entry.ColumnPlayed;
                    sb.AppendLine(csvRow);
                }
            }

            File.WriteAllText(@"C:\Users\rbenn\source\repos\Connect4AI\GameLogs\GamesTab.csv",sb.ToString());
        }


    }
}
