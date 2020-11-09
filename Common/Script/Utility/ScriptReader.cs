using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Common.Script.Listeners;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.Utility
{
    public static class ScriptReader
    {
        public static IParseTree MakeParseTree(string script, out List<Error> errors)
        {
            var inputStream = new AntlrInputStream(script);
            var lexer = new ViGaSLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new ViGaSParser(tokenStream);
            var syntaxErrorListener = new SyntaxErrorListener();
            parser.AddErrorListener(syntaxErrorListener);
            var context = parser.script();

            errors = syntaxErrorListener.errors;
            return context;
        }
    }
}
