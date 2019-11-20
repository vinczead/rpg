using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using GameScript.SymbolTable;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using GameScript.Model;
using GameScript.Visitors;
using GameModel.Models;

namespace GameScript
{
    public class Executer
    {
        public static IParseTree ReadAST(string fileName)
        {
            var code = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, fileName));
            var inputStream = new AntlrInputStream(code);
            var lexer = new GameScriptLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new GameScriptParser(tokenStream);
            var context = parser.program();
            return context;
        }

        private static List<Function> ReadFunctionList(string fileName)
        {
            var json = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, fileName));
            return JsonConvert.DeserializeObject(json) as List<Function>;
        }

        private static IParseTree ParseStatement(string statement)
        {
            var inputStream = new AntlrInputStream(statement);
            var lexer = new GameScriptLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new GameScriptParser(tokenStream);
            var context = parser.statement();
            return context;
        }

        private static IParseTree ParseScript(string script)
        {
            var inputStream = new AntlrInputStream(script);
            var lexer = new GameScriptLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new GameScriptParser(tokenStream);
            var context = parser.program();
            return context;
        }

        public static void ExecuteStatement(string statement, World world, GameObject gameObject)
        {
            var parseTree = ParseStatement(statement);
            var executionVisitor = new ExecutionVisitor(world, gameObject);
            executionVisitor.Visit(parseTree);
        }

        public static void ExecuteRunBlock(World world, GameObject gameObject, string runBlockType)
        {
            var parseTree = ParseScript(gameObject.Base.Script);
            var executionVisitor = new ExecutionVisitor(world, gameObject);
            executionVisitor.Visit(parseTree);
        }

        /*private static void Main(string[] args)
        {
            var ast = ReadAST("example.gsc");
            var fun = ReadFunctionList("Language.json");    //esetleg lehetne saját nyelvből, avalonedit: szintaxkiemelő
            var gameScriptVisitor = new GameScriptVisitor(); //semantic highlight
            gameScriptVisitor.Visit(ast);

            var ast = ParseStatement("Set Var1 To 5 > 2");
            var executionVisitor = new ExecutionVisitor(null, null);
            executionVisitor.Visit(ast);

            Console.ReadLine();
        }*/
    }
}
