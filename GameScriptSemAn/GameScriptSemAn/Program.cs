using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using GameScriptSemAn.SymbolTable;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using GameScriptSemAn.Model;
using GameScriptSemAn.Execution;
using Rpg.Models;

namespace GameScriptSemAn
{
    internal class Program
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

        private static void ExecuteStatement(string statement, World world)
        {

        }

        private static void Main(string[] args)
        {
            /*var ast = ReadAST("example.gsc");
            var fun = ReadFunctionList("Language.json");    //esetleg lehetne saját nyelvből, avalonedit: szintaxkiemelő
            var gameScriptVisitor = new GameScriptVisitor(); //semantic highlight
            gameScriptVisitor.Visit(ast);*/

            var ast = ParseStatement("Set Var1 To 5 < 2");
            var executionVisitor = new GameScriptExecutionVisitor();
            executionVisitor.Visit(ast);

            Console.ReadLine();
        }
    }
}
