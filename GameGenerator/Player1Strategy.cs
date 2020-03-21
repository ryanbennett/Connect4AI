using Connect4AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameGenerator
{
    public class Player1Strategy
    {

        private const int PLAYERNUM = 1;
        public int Run(Board board)
        {

            var zzDetector = new ZugzwangDetector();
            var hasZZ = zzDetector.DoesPlayerHaveZugzwang(PLAYERNUM, board);





        }
    }
}
