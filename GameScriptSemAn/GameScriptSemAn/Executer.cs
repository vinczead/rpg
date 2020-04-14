using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using GameScript.Model;
using GameScript.Visitors;
using System.Linq;
using GameScript.Listeners;
using GameScript.Models.InstanceClasses;
using GameScript.Models;

namespace GameScript
{
    public class Executer
    {
        public static IParseTree ReadAST(string script, out List<Error> syntaxErrors)
        {
            var inputStream = new AntlrInputStream(script);
            var lexer = new ViGaSLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new ViGaSParser(tokenStream);
            var syntaxErrorListener = new SyntaxErrorListener();
            parser.AddErrorListener(syntaxErrorListener);
            var context = parser.script();
            syntaxErrors = syntaxErrorListener.errors;
            return context;
        }

        public static List<string> CheckErrors(List<string> scripts)
        {
            List<string> errors = new List<string>();
            var errorVisitor = new ErrorVisitor();

            foreach (var script in scripts)
            {
                List<Error> syntaxErrors;
                var tree = ReadAST(script, out syntaxErrors);
                errorVisitor.Visit(tree);
                
                if (errorVisitor.errors.Count > 0 || syntaxErrors.Count > 0)
                {
                    errors.AddRange(syntaxErrors.Select(e => e.ToString()));
                    errors.AddRange(errorVisitor.errors.Select(e => e.ToString()));
                }
            }

            return errors;
        }

        public static World BuildWorld(List<string> scripts)
        {
            var worldBuilder = new WorldBuilderVisitor();
            foreach (var script in scripts)
            {
                worldBuilder.Visit(ReadAST(script, out _), script);   
            }

            return worldBuilder.World;
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

        public static void ExecuteVariableDeclaration(GameObjectInstance instance)
        {
            /*if (gameObject.Base.Script != "")
            {
                var parser = GetParser(gameObject.Base.Script);
                var varBlockContext = parser.program().variablesBlock();
                if (varBlockContext != null)
                {
                    var executionVisitor = new ExecutionVisitor(gameObject);
                    executionVisitor.Visit(varBlockContext);
                }
            }*/
        }

        public static void ExecuteRunBlock(ThingInstance thing, string runBlockType)
        {
            if (gameObject.Base.Script != "")
            {
                var parser = GetParser(gameObject.Base.Script);
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
            }
        }
    }
}
