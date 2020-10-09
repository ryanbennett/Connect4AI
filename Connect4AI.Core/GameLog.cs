using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Connect4AI.Core
{
    public class GameLog : IGameLog
    {


        public List<GameLogEntry> Log { get; set; } = new List<GameLogEntry>();
        public int Winner { get; set; }

        public void Add(GameLogEntry entry)
        {
            Log.Add(entry);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText(@"C:\Users\rbenn\source\repos\Connect4AI\GameLogs\" + Guid.NewGuid() + ".json", json);
        }

        public static GameLog Load(string filePath)
        {
            var text = File.ReadAllText(filePath);
            var log = JsonConvert.DeserializeObject<GameLog>(text);

            return log;
        }
    }

    public class GameLogEntry
    {

        public int[,] BoardBeforeMove { get; set; }
        public int ColumnPlayed { get; set; }
        public int PlayerNumber { get; set; }
    }
}
