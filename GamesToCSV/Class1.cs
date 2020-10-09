using Connect4AI.Core;
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
            var files = new DirectoryInfo(@"C:\Users\rbenn\source\repos\Connect4AI\GameLogs").GetFiles();

            var csv = File.CreateText(@"C:\Users\rbenn\source\repos\Connect4AI\GameLogs\Games.csv");
            csv.Dispose();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("i0,i1,i2,i3,i4,i5,i6,i7,i8,i9,i10,i11,i12,i13,i14,i15,i16,i17,i18,i19,i20,i21,i22,i23,i24,i25,i26,i27,i28,i29,i30,i31,i32,i33,i34,i35,i36,i37,i38,i39,i40,i41,indexPlayed,turnNum");
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


                var turnCount = 0;               
                foreach(var entry in gameLog.Log.Where(e => e.PlayerNumber == 2))
                {
                    
                    var csvRow = string.Empty;
                    for (var r=0; r< 6; r++)
                    {
                        for(var c = 0; c < 7; c++)
                        {
                            double value = (double)entry.BoardBeforeMove[r, c];
                           
                            if(value == 2)
                            {
                                value = -1;
                            }

                            csvRow += value + ",";
                        }
                    }

                    turnCount++;
                    csvRow += entry.ColumnPlayed;
                    csvRow += "," + turnCount;
                    sb.AppendLine(csvRow);
                 
                }
            }

            File.WriteAllText(@"C:\Users\rbenn\source\repos\Connect4AI\GameLogs\Games.csv",sb.ToString());
        }


    }
}
