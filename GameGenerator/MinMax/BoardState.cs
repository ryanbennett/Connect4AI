using MinMaxSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameGenerator.MinMax
{
    
        public class Connect4State : IDeterministicState
        {
            public const int RowCount = 6;
            public const int ColCount = 7;

            public Connect4State(Player[,] board, Player turn)
            {
                Board = board;
                Turn = turn;
            }

            public Player[,] Board { get; }
            public Player Turn { get; }

            public IEnumerable<IState> GetNeighbors()
            {
                if (BoardEvaluator.IsWin(Board, Turn.GetReversePlayer()))
                    return new List<IDeterministicState>();

                var result = new List<Connect4State>();
                for (int col = 0; col < ColCount; col++)
                {
                    var newState = AddPieceTo(col);
                    if (newState != null)
                        result.Add(newState);
                }

                return result;
            }

            public double Evaluate(int depth, List<IState> passedThroughStates) =>
                BoardEvaluator.Evaluate(Board);

            public Connect4State AddPieceTo(int col)
            {
                var newBoard = (Player[,])Board.Clone();
                for (int row = 0; row < RowCount; row++)
                    if (newBoard[row,col] == Player.Empty)
                    {
                        newBoard[row,col] = Turn;
                        return new Connect4State(newBoard, Turn.GetReversePlayer());
                    }

                return null;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Connect4State connect4State))
                    return false;

                if (Turn != connect4State.Turn)
                    return false;

                for (var row = 0; row < RowCount; row++)
                    for (var col = 0; col < ColCount; col++)
                        if (Board[row, col] != connect4State.Board[row, col])
                            return false;

                return true;
            }

            public override int GetHashCode()
            {
                int sum = 0;

                for (var row = 0; row <RowCount; row++)
                    for (var col = 0; col < ColCount; col++)
                        sum += GetValue(Board[row, col]) * (int)Math.Pow(3, row + col);

                return sum + (int)Turn * (int)Math.Pow(3, RowCount* ColCount);
            }

            private int GetValue(Player player)
            {
                switch (player)
                {
                    case Player.Min:
                        return 1;
                    case Player.Max:
                        return 2;
                    default:
                        return 0;
                }
            }

            public override string ToString()
            {
                var builder = new StringBuilder();

                for (int row = 0; row < RowCount; row++)
                {
                    for (int col = 0; col < ColCount; col++)
                        builder.Append(GetValue(Board[row, col]) + " ");
                    builder.Append("#" + Environment.NewLine);
                }

                return builder.ToString();
            }
        }
    
}
