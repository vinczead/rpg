using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using GameScript.Visitors;
using System.Linq;
using GameScript.Listeners;
using GameScript.Models.InstanceClasses;
using GameScript.Models;
using GameScript.Models.Script;

namespace GameScript
{
    public class Executer
    {
        public static IParseTree ReadAST(string script, out List<Error> errors)
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

        private static IParseTree ParseStatement(string statement)
        {
            var inputStream = new AntlrInputStream(statement);
            var lexer = new ViGaSLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new ViGaSParser(tokenStream);
            var context = parser.statement();
            return context;
        }

        private static ViGaSParser GetParser(string script)
        {
            var inputStream = new AntlrInputStream(script);
            var lexer = new ViGaSLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new ViGaSParser(tokenStream);
            return parser;
        }

        public static void ExecuteStatement(ThingInstance thing, string statement)
        {
            //var parseTree = ParseStatement(statement);
            //var executionVisitor = new ExecutionVisitorOld(gameObject);
            //executionVisitor.Visit(parseTree);
        }

        //public static void ExecuteRunBlock(ThingInstance thing, string runBlockType)
        //{
            /*if (thing.Base.Script != "")
            {
                var parser = GetParser(thing.Base.Script);
                var runBlocksContext = parser.program().runBlock();
                if (runBlocksContext != null)
                {
                    var runBlock = runBlocksContext.FirstOrDefault(r => r.eventTypeName().GetText() == runBlockType);
                    if (runBlock != null)
                    {
                        var executionVisitor = new ExecutionVisitor(gameObject);
                        executionVisitor.Visit(runBlock);
                    }
                }
            }*/
        //}
    }
}
