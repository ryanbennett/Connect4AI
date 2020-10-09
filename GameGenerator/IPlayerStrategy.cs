using Connect4AI;

namespace GameGenerator
{
    public interface IPlayerStrategy
    {
        int Run(Board board, int numToWin, int playerNumber, int searchDepth = 8);
    }
}