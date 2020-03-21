using System;
using System.Collections.Generic;
using System.Linq;

public class Group
{
    public int Player { get; set; }


    public List<Tuple<int, int, int>> Coords { get; set; } = new List<Tuple<int, int, int>>(4);

    public bool IsWinner()
    {
        return !Coords.Any(t => t.Item3 == 0);
    }

}