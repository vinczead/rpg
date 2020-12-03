using Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Common.Utility
{
    public class InstanceCoordinateComparer : IComparer<ThingInstance>
    {
        public int Compare([AllowNull] ThingInstance x, [AllowNull] ThingInstance y)
        {
            var yComparisonResult = x.Position.Y.CompareTo(y.Position.Y);

            if(yComparisonResult == 0)
                return x.Position.X.CompareTo(y.Position.X);

            return yComparisonResult;
        }
    }
}
