using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameScript.ViGaSParser;

namespace GameScript.Visitors
{
    public class InvalidTypeException : Exception
    {
        public InvalidTypeException(ExpressionContext context, Type expectedType)
            : base($"At line #{context.start.Line}, column #{context.start.Column}: Invalid Type (expected #{expectedType.Name})")
        {
        }
    }
}
