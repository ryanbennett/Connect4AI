using Connect4AI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameGenerator
{
    public static class BoardJson
    {
        public static void Serialize(Board board)
        {
            var json = JsonConvert.SerializeObject(board);
            File.WriteAllText(@"C:\Users\rbenn\source\repos\Connect4AI\SavedBoards\" + board.Name + ".json",json);
        }

        public  static Board Deserialize(string name)
        {
            var json = File.ReadAllText(@"C:\Users\rbenn\source\repos\Connect4AI\SavedBoards\" + name + ".json");
            var board = JsonConvert.DeserializeObject<Board>(json);
            return board;
        }
    }
}
