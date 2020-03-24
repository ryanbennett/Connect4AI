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

        public List<GameLogEntry> Log { get => log; set => log = value; }

        public void Add(GameLogEntry entry)
        {
            Log.Add(entry);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(Log);
            File.WriteAllText(@"C:\Users\rbenn\source\repos\Connect4AI\GameLogs\" + Guid.NewGuid()+ ".json", json);
        }

        public static GameLog Load(string filePath)
        {
            var text = File.ReadAllText(filePath);
            var entries = JsonConvert.DeserializeObject<List<GameLogEntry>>(text);
            var log = new GameLog();
            log.Log = entries;
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
