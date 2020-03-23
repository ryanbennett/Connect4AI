using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Connect4AI
{
    public class PositionEqualityComparer : IEqualityComparer<Position>
    {
        public bool Equals([AllowNull] Position x, [AllowNull] Position y)
        {
            return x.Col == y.Col && x.Row == y.Row;
        }

        public int GetHashCode([DisallowNull] Position obj)
        {
            return obj.GetHashCode();
        }
    }
}
