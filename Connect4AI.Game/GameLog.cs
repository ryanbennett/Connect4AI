using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Connect4AI.Game
{
    public class GameLog
    {
        List<GameLogEntry> log = new List<GameLogEntry>();

        public void Add(GameLogEntry entry)
        {
            log.Add(entry);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(log);
            File.WriteAllText(@"C:\Users\rbenn\source\repos\Connect4AI\GameLogs\" + Guid.NewGuid()+ ".json", json);
        }
    }

    public class GameLogEntry
    {

        public int[,] BoardBeforeMove { get; set; }
        public int ColumnPlayed { get; set; }
        public int PlayerNumber { get; set; }
    }
}
