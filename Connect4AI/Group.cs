using Connect4AI;
using System;
using System.Collections.Generic;
using System.Linq;

public class Group
{
    public int Player { get; set; }


    public List<Position> Coords { get; set; } = new List<Position>();

    public bool IsWinner()
    {
        return !Coords.Any(t => t.Mark == 0);
    }

}