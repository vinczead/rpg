//using Antlr4.Runtime;
//using Antlr4.Runtime.Misc;
//using GameScript.SymbolTable;
//using System;

//namespace GameScript.Visitors
//{
//    internal class TypeCheckVisitor : GameScriptBaseVisitor<object>
//    {
//        private Env env;
//        private TypeSystem typeSystem;

//        public TypeCheckVisitor(Env defaultEnv = null)
//        {
//            env = defaultEnv ?? new Env();
//            typeSystem = new TypeSystem();
//        }

//        public override object VisitVariablesBlock([NotNull] GameScriptParser.VariablesBlockContext context)
//        {
//            env = new Env(env, "Variables Scope");
//            var result = base.VisitVariablesBlock(context);

//            Console.WriteLine(env.ToString());
//            return result;
//        }

//        public override object VisitExpression([NotNull] GameScriptParser.ExpressionContext context)
//        {
//            var t = GetType(context);
//            Console.WriteLine($"Type Of {context.GetText()} Is {t.Name}");

//            return t;
//        }

//        public override object VisitIfStatement([NotNull] GameScriptParser.IfStatementContext context)
//        {
//            var type = GetType(context.expression());

//            if (!type.InheritsFrom(typeSystem.BOOLEAN))
//                Console.WriteLine($"{GetPosition(context.expression())}: expression must be of type ${typeSystem.BOOLEAN.Name}.");

//            return base.VisitIfStatement(context);
//        }

//        /*public override object VisitRepeatStatement([NotNull] GameScriptParser.RepeatStatementContext context)
//        {
//            var type = GetType(context.expression());

//            if (!type.InheritsFrom(typeSystem.BOOLEAN))
//                Console.WriteLine($"{GetPosition(context.expression())}: expression must be of type ${typeSystem.BOOLEAN.Name}.");

//            return base.VisitRepeatStatement(context);
//        }

//        public override object VisitWhileStatement([NotNull] GameScriptParser.WhileStatementContext context)
//        {
//            var type = GetType(context.expression());

//            if (!type.InheritsFrom(typeSystem.BOOLEAN))
//                Console.WriteLine($"{GetPosition(context.expression())}: expression must be of type ${typeSystem.BOOLEAN.Name}.");

//            return base.VisitWhileStatement(context);
//        }*/

//        public override object VisitAssignmentStatement([NotNull] GameScriptParser.AssignmentStatementContext context)
//        {
//            var expressionType = GetType(context.expression());
//            var varName = context.varExp().GetText();
//            var symbol = GetVariableFromScope(context.varExp(), varName);

//            if (symbol != null)
//            {
//                var varType = symbol.Type;
//                if (!expressionType.InheritsFrom(varType))
//                    Console.WriteLine($"{GetPosition(context)}: '{expressionType.Name}' is not assignable to variable {varName} of type {varType}.");
//            }

//            return base.VisitAssignmentStatement(context);
//        }

//        public override object VisitVariableDeclaration([NotNull] GameScriptParser.VariableDeclarationContext context)
//        {
//            var varName = context.varName().GetText();
//            var typeName = context.typeName().GetText();
//            var varType = typeSystem[typeName];
//            if (context.expression() == null)
//            {
//                var symbol = new Symbol(varName, varType);
//                AddVariableToScope(context.varName(), symbol);
//            }
//            else
//            {
//                var expressionType = GetType(context.expression());
//                if (!expressionType.InheritsFrom(varType))
//                    Console.WriteLine($"{GetPosition(context.expression())}: expression of type {expressionType} is not assignable to variable {varName} of type {varType}.");
//                else
//                {
//                    var symbol = new Symbol(context.varName().GetText(), varType);
//                    AddVariableToScope(context.varName(), symbol);
//                }
//            }

//            return base.VisitVariableDeclaration(context);
//        }

//        public override object VisitProgram([NotNull] GameScriptParser.ProgramContext context)
//        {
//            var result = base.VisitProgram(context);
//            env = env.Previous;

//            Console.WriteLine($"End of Script {context.scriptStart().scriptName().GetText()}.");
//            Console.WriteLine(env.ToString());
//            return result;
//        }

//        public override object VisitFunctionCallStatement([NotNull] GameScriptParser.FunctionCallStatementContext context)
//        {

//            return base.VisitFunctionCallStatement(context);
//        }

//        private static string GetPosition(ParserRuleContext context)
//        {
//            return $"At line #{context.start.Line}, column #{context.start.Column}";
//        }

//        private void CheckFunctionValidity(GameScriptParser.FunctionCallStatementContext context)
//        {

//        }

//        private Symbol GetVariableFromScope(ParserRuleContext context, string varName)
//        {
//            var symbol = env[varName];
//            if (symbol != null) return symbol;
//            Console.WriteLine($"{GetPosition(context)}: '{varName}' is not defined.");
//            return null;

//        }

//        private void AddVariableToScope(ParserRuleContext context, Symbol symbol)
//        {
//            try
//            {
//                env[symbol.Name] = symbol;
//            }
//            catch (NameAlreadyDefinedException e)
//            {
//                Console.WriteLine($"{GetPosition(context)}: {e.Message}");
//            }
//        }

//        /*private Type GetType(GameScriptParser.ExpressionContext context)
//        {
//            Type type = null;
//            if (context.varName() != null)
//            {
//                Symbol symbol = null;
//                var varName = context.varName();
//                symbol = GetVariableFromScope(varName, varName.GetText());
//                type = symbol?.Type ?? typeSystem.ERROR;
//            }

//            if (context.NUMBER() != null)
//                type = typeSystem.NUMBER;

//            if (context.BOOLEAN() != null)
//                type = typeSystem.BOOLEAN;

//            if (context.STRING() != null)
//                type = typeSystem.STRING;

//            if (context.expression().Length == 1)
//                type = GetType(context.expression(0));

//            if (context.expression().Length == 2)
//            {
//                var left = GetType(context.expression(0));
//                if (left == typeSystem.ERROR)
//                    return typeSystem.ERROR;

//                var right = GetType(context.expression(1));
//                if (right == typeSystem.ERROR)
//                    return typeSystem.ERROR;

//                var op = context.@operator();

//                if (op.mathOperator() != null)
//                {
//                    if (left != typeSystem.NUMBER || right != typeSystem.NUMBER)
//                        type = typeSystem.ERROR;
//                    else
//                        type = typeSystem.NUMBER;
//                }
//                if (op.logicalOperator() != null)
//                {
//                    if (left != typeSystem.BOOLEAN || right != typeSystem.BOOLEAN)
//                        type = typeSystem.ERROR;
//                    else
//                        type = typeSystem.BOOLEAN;
//                }
//                if (op.compOperator() != null)
//                {
//                    if (left != right)
//                        type = typeSystem.ERROR;
//                    else
//                        type = typeSystem.BOOLEAN;
//                }
//                if (type == typeSystem.ERROR)
//                    Console.WriteLine($"{GetPosition(context)}: operator '{op.GetText()}' cannot be used on types '{left?.Name}' and '{right?.Name}'.");
//            }


//            if (context.functionCallStatement() != null)
//            {
//                //todo get function value
//            }

//            return type;
//        }
//        */

//    }
//}
