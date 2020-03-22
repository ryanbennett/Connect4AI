using Connect4AI;
using System;
using System.Collections.Generic;
using System.Linq;

public class Group
{
    public int Player { get; set; }


    public List<Position> Coords { get; set; } = new List<Position>(4);

    public bool IsWinner()
    {
        return !Coords.Any(t => t.Item3 == 0);
    }

}