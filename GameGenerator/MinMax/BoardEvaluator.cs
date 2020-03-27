using MinMaxSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameGenerator.MinMax
{
    public static class BoardEvaluator
    {
        public const int MaxEvaluation = 100000;
        public const int MinEvaluation = -100000;

        public static bool IsWin(Player[,] board, Player player)
        {
            return IsWinInDirection(board, 0, 1, player) ||
                IsWinInDirection(board, 1, 0, player) ||
                IsWinInDirection(board, 1, 1, player) ||
                IsWinInDirection(board, -1, 1, player);
        }

        private static bool IsWinInDirection(Player[,] board, int direction1, int direction2, Player player)
        {
            for (int row = 0; row < Connect4State.RowCount; row++)
                for (int col = 0; col < Connect4State.ColCount; col++)
                    if (PiecesInRow(board, row, col, direction1, direction2, player) == 4)
                        return true;

            return false;
        }

        public static int Evaluate(Player[,] board)
        {
            var maxPoints = EvaluateDirection(board, 0, 1, Player.Max) +
                            EvaluateDirection(board, 1, 0, Player.Max) +
                            EvaluateDirection(board, 1, 1, Player.Max) +
                            EvaluateDirection(board, -1, 1, Player.Max);
            if (maxPoints >= MaxEvaluation)
                return MaxEvaluation;

            var minPoints = EvaluateDirection(board, 0, 1, Player.Min) +
                            EvaluateDirection(board, 1, 0, Player.Min) +
                            EvaluateDirection(board, 1, 1, Player.Min) +
                            EvaluateDirection(board, -1, 1, Player.Min);
            if (minPoints >= MaxEvaluation)
                return MinEvaluation;

            return maxPoints - minPoints;
        }

        private static int EvaluateDirection(Player[,] board, int direction1, int direction2, Player player)
        {
            var sum = 0;
            for (int row = 0; row < Connect4State.RowCount; row++)
                for (int col = 0; col < Connect4State.ColCount; col++)
                {
                    var piecesInRow = PiecesInRow(board, row, col, direction1, direction2, player);
                    if (piecesInRow == 4)
                        return MaxEvaluation;
                    if (piecesInRow == 3)
                        sum += 20;
                    if (piecesInRow == 2)
                        sum += 10;
                    sum += piecesInRow;
                }

            return sum;
        }

        private static int PiecesInRow(Player[,] board, int startX, int startY, int direction1, int direction2, Player player)
        {
            var pieces = 0;
            for (int piecesInARow = 0; piecesInARow < 4; piecesInARow++)
            {
                // out of range
                if (startX + piecesInARow * direction1 >= Connect4State.RowCount || startY + piecesInARow * direction2 >= Connect4State.ColCount)
                    return 0;
                if (startX + piecesInARow * direction1 < 0 || startY + piecesInARow * direction2 < 0)
                    return 0;
                if (board[startX + piecesInARow * direction1, startY + piecesInARow * direction2] == player)
                    pieces++;
                else if (board[startX + piecesInARow * direction1, startY + piecesInARow * direction2] != Player.Empty)
                    return 0;
            }

            return pieces;
        }
    }
}
