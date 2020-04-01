using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using GameScript.Model;
using GameScript.Visitors;
using GameModel.Models;
using GameModel.Models.InstanceInterfaces;
using System.Linq;
using GameScript.Listeners;

namespace GameScript
{
    public class Executer
    {
        public static IParseTree ReadASTFromFile(string fileName)
        {
            var code = File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, fileName));
            return ReadAST(code, out _);
        }

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

        public static ErrorVisitor CheckErrors(IParseTree tree)
        {
            var typeVisitor = new ErrorVisitor();

            typeVisitor.Visit(tree);

            return typeVisitor;
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

        public static void ExecuteStatement(IGameWorldObject gameObject, string statement)
        {
            var parseTree = ParseStatement(statement);
            var executionVisitor = new ExecutionVisitor(gameObject);
            executionVisitor.Visit(parseTree);
        }

        public static void ExecuteVariableDeclaration(IGameWorldObject gameObject)
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

        public static void ExecuteRunBlock(IGameWorldObject gameObject, string runBlockType) //todo: take context based Variables as parameter
        {
            /*if (gameObject.Base.Script != "")
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
            }*/
        }
    }
}
