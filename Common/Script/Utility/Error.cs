﻿using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Script.Utility
{
    public class Error
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string Message { get; set; }

        public ErrorSeverity Severity { get; set; }

        public Error(ParserRuleContext context, string message, ErrorSeverity severity = ErrorSeverity.Error)
        {
            Line = context.start.Line;
            Column = context.start.Column;
            Message = message;
            Severity = severity;
        }

        public Error(IToken token, string message, ErrorSeverity severity = ErrorSeverity.Error)
        {
            Line = token.Line;
            Column = token.Column;
            Message = message;
            Severity = severity;
        }

        public Error(int line, int column, string message, ErrorSeverity severity = ErrorSeverity.Error)
        {
            Line = line;
            Column = column;
            Message = message;
            Severity = severity;
        }

        public override string ToString()
        {
            return $"{Severity:f} at line {Line}, column {Column}: {Message}";
        }
    }

    public enum ErrorSeverity
    {
        Message,
        Warning,
        Error
    }
}
