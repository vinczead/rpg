using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Models.Script
{
    public class Error
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string Message { get; set; }

        public Error(ParserRuleContext context, string message)
        {
            Line = context.start.Line;
            Column = context.start.Column;
            Message = message;
        }

        public Error(int line, int column, string message)
        {
            Line = line;
            Column = column;
            Message = message;
        }

        public override string ToString()
        {
            return $"at line {Line}, column {Column}: {Message}";
        }
    }
}
