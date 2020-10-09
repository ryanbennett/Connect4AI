using System.Collections.Generic;

namespace Connect4AI.Core
{
    public interface IGameLog
    {
        List<GameLogEntry> Log { get; set; }
        int Winner { get; set; }

        void Add(GameLogEntry entry);
        void Save();
    }
}