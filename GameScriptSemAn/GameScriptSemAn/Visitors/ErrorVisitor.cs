using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using GameScript.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using static GameScript.ViGaSParser;

namespace GameScript.Visitors
{
    public class ErrorVisitor : ViGaSBaseVisitor<object>
    {
        Env env;
        TypeSystem typeSystem;
        public List<Error> errors;

        public override object VisitScript([NotNull] ScriptContext context)
        {
            env = new Env();
            typeSystem = new TypeSystem();
            typeSystem.ReadTypes(@"C:\Users\Ady\source\repos\Rpg\GameScriptSemAn\GameScriptSemAn\builtintypes.json");
            errors = new List<Error>();

            return base.VisitScript(context);
        }

        public override object VisitBaseDefinition([NotNull] BaseDefinitionContext context)
        {
            var baseClass = context.baseHeader().baseClass();
            var baseId = context.baseHeader().baseId();

            Model.Type baseClassType = typeSystem["ErrorType"];
            try
            {
                baseClassType = typeSystem[baseClass.GetText()];
            }
            catch (KeyNotFoundException e)
            {
                errors.Add(new Error(baseClass, e.Message));
            }
            if (!baseClassType.InheritsFrom(typeSystem["GameObject"]))
                errors.Add(new Error(baseClass, $"Type {baseClassType} is not valid here (must inherit from {typeSystem["GameObject"]})."));

            //TODO: add newly defined type {baseId.GetText()} to type system.

            env = new Env(env, baseId.GetText(), baseClassType);

            foreach (var prop in baseClassType.Properties)
                AddSymbolToEnv(baseClass, new Symbol(prop.Name, prop.Type));

            var result = base.VisitBaseDefinition(context);

            env = env.Previous;

            return result;
        }

        public override object VisitInstanceDefinition([NotNull] InstanceDefinitionContext context)
        {
            var baseId = context.instanceHeader().baseId();
            var instanceId = context.instanceHeader().instanceId();

            //TODO: check whether {baseId.GetText()} is a defined base type.

            env = new Env(env, instanceId.GetText());
            var result = base.VisitInstanceDefinition(context);

            env = env.Previous;

            return result;
        }

        public override object VisitRunBlock([NotNull] RunBlockContext context)
        {
            var eventName = context.eventTypeName();

            if (!env.Type.Events.Contains(eventName.GetText()))
                errors.Add(new Error(eventName, $"Event {eventName.GetText()} is not defined on {env.Type}."));

            return base.VisitRunBlock(context);
        }

        public override object VisitIfStatement([NotNull] IfStatementContext context)
        {
            var type = GetType(context.expression());

            if (!type.InheritsFrom(typeSystem["Boolean"]))
                errors.Add(new Error(context, $"Expression type must be {typeSystem["Boolean"]}"));

            return base.VisitIfStatement(context);
        }

        public override object VisitRepeatStatement([NotNull] RepeatStatementContext context)
        {
            var type = GetType(context.expression());

            if (!type.InheritsFrom(typeSystem["Boolean"]))
                errors.Add(new Error(context, $"Expression type must be {typeSystem["Boolean"]}"));

            return base.VisitRepeatStatement(context);
        }

        public override object VisitWhileStatement([NotNull] WhileStatementContext context)
        {
            var type = GetType(context.expression());

            if (!type.InheritsFrom(typeSystem["Boolean"]))
                errors.Add(new Error(context, $"Expression type must be {typeSystem["Boolean"]}"));

            return base.VisitWhileStatement(context);
        }

        public override object VisitVariableDeclaration([NotNull] VariableDeclarationContext context)
        {
            //check whether expression after WithValue matches type
            var varName = context.varName().GetText();

            Model.Type varType = typeSystem["ErrorType"];

            try
            {
                varType = typeSystem[context.typeName().GetText()];
            }
            catch (KeyNotFoundException e)
            {
                errors.Add(new Error(context.typeName(), e.Message));
            }

            AddSymbolToEnv(context, new Symbol(varName, varType, context.PARAMETER() != null));

            if (context.expression() != null)
            {
                var expressionType = GetType(context.expression());

                var symbol = GetSymbolFromEnv(context, varName);

                if (symbol != null)
                {
                    if (!expressionType.InheritsFrom(varType))
                    {
                        errors.Add(new Error(context.expression(), $"Type mismatch: expression of type {expressionType} cannot be assigned to a variable of type {varType}"));
                    }
                }
            }

            return base.VisitVariableDeclaration(context);
        }

        public override object VisitAssignmentStatement([NotNull] AssignmentStatementContext context)
        {
            var varName = context.path().GetText();                 //TODO: this should check whole path
            var expressionType = GetType(context.expression());

            var symbol = GetSymbolFromEnv(context.path(), varName);

            if (symbol != null)
            {
                if (symbol.Readonly)
                    errors.Add(new Error(context.expression(), $"Invalid assignment: {symbol.Name} is read only."));
                else
                {
                    var varType = symbol.Type;

                    if (!expressionType.InheritsFrom(varType))
                    {
                        errors.Add(new Error(context.expression(), $"Type mismatch: expression of type {expressionType} cannot be assigned to a variable of type {varType}"));
                    }
                }
            }
            return base.VisitAssignmentStatement(context);
        }

        public override object VisitAssignmentStatementBlock([NotNull] AssignmentStatementBlockContext context)
        {
            //todo: iterate over each assignment, and check whether expression type matches var type


            return base.VisitAssignmentStatementBlock(context);
        }

        public override object VisitFunctionCallStatement([NotNull] FunctionCallStatementContext context)
        {
            //get type of path, check whether function exists and parameter types match

            var varName = context.path().GetText();                 //TODO: this should check whole path
            var symbol = GetSymbolFromEnv(context.path(), varName);

            if (symbol != null && symbol.Type != typeSystem["ErrorType"])
            {
                var funcCtx = context.functionName();

                var function = symbol.Type.Functions.FirstOrDefault(f => f.Name == funcCtx.GetText());
                if (function != null)
                {
                    var paramsCtx = context.functionParameterList();
                    var requiredParamCount = function.Parameters.Count;
                    var actualParamCount = paramsCtx?.expression()?.Length ?? 0;

                    if (actualParamCount == requiredParamCount)
                    {
                        for (int i = 0; i < function.Parameters.Count; i++)
                        {
                            if (function.Parameters[i].Type != GetType(paramsCtx.expression(i)))
                                errors.Add(new Error(funcCtx, $"Type mismatch: The function {function} expects {function.Parameters[i].Type} as parameter {i + 1}, not {GetType(paramsCtx.expression(i))}."));
                        }
                    }
                    else
                        errors.Add(new Error(funcCtx, $"The function {function} requires {requiredParamCount} parameter(s), not {actualParamCount}."));
                }
                else
                {
                    errors.Add(new Error(funcCtx, $"The function {funcCtx.GetText()} is not defined on {symbol.Type}."));
                }
            }

            return base.VisitFunctionCallStatement(context);
        }

        private void AddSymbolToEnv(ParserRuleContext context, Symbol symbol)
        {
            try
            {
                env[symbol.Name] = symbol;
            }
            catch
            {
                errors.Add(new Error(context, $"{symbol.Name} is already defined."));
            }
        }

        private Symbol GetSymbolFromEnv(ParserRuleContext context, string symbolName)
        {
            var symbol = env[symbolName];
            if (symbol != null)
                return symbol;

            errors.Add(new Error(context, $"{symbolName} does not exist in this context."));
            return null;
        }

        private Model.Type GetType(ExpressionContext context)
        {
            if (context is ParenExpressionContext)
                return GetType((context as ParenExpressionContext).expression());

            if (context is PathExpressionContext)
            {
                var ctx = context as PathExpressionContext;
                var variable = GetSymbolFromEnv(ctx, ctx.path().varPath().varName()[0].GetText());
                return variable.Type;
            }

            /*if (context is RefExpressionContext)
                return GetTypeOfRefExpression(context as RefExpressionContext);*/

            if (context is StringExpressionContext)
                return typeSystem["String"];

            if (context is BoolExpressionContext)
                return typeSystem["Boolean"];

            if (context is NumberExpressionContext)
                return typeSystem["Number"];

            if (context is NullExpressionContext)
                return typeSystem["NullType"];

            if (context is NotExpressionContext)
            {
                var type = GetType((context as NotExpressionContext).expression());

                if (!type.InheritsFrom(typeSystem["Boolean"]))
                {
                    errors.Add(new Error(context, $"Type mismatch: expression type must be {typeSystem["Boolean"]}"));
                    return typeSystem["ErrorType"];
                }

                return type;
            }

            if (context is CompExpressionContext)
            {
                var ctx = context as CompExpressionContext;
                var leftType = GetType(ctx.left);
                var rightType = GetType(ctx.right);
                var @operator = ctx.compOperator();

                if (!leftType.InheritsFrom(rightType) && !rightType.InheritsFrom(leftType))
                {
                    errors.Add(new Error(ctx, $"Illegal operator: operator {@operator.GetText()} cannot be applied to types {leftType} and {rightType}"));
                    return typeSystem["ErrorType"];
                }

                //operators <, >, <=, >= are only defined on numbers
                if (@operator.EQ() == null && @operator.NEQ() == null && !leftType.InheritsFrom(typeSystem["Number"]))
                {
                    errors.Add(new Error(ctx, $"Illegal operator: operator {@operator.GetText()} cannot be applied to types {leftType} and {rightType}"));
                    return typeSystem["ErrorType"];
                }

                return typeSystem["Boolean"];
            }

            if (context is AdditiveExpressionContext)
            {
                var ctx = context as AdditiveExpressionContext;
                var leftType = GetType(ctx.left);
                var rightType = GetType(ctx.right);
                var @operator = ctx.additiveOperator();

                if (leftType.InheritsFrom(typeSystem["String"]) && @operator.PLUS() != null)
                    return typeSystem["String"];

                if (leftType == rightType && leftType.InheritsFrom(typeSystem["Number"]))
                    return typeSystem["Number"];

                errors.Add(new Error(@operator, $"Illegal operator: operator {@operator.GetText()} cannot be applied to types {leftType} and {rightType}"));// (TODO: check inheritance too)
                return typeSystem["ErrorType"];
            }

            if (context is MultiplExpressionContext)
            {
                var ctx = context as MultiplExpressionContext;
                var leftType = GetType(ctx.left);
                var rightType = GetType(ctx.right);

                if (leftType == rightType && leftType == typeSystem["Number"])
                    return typeSystem["Number"];

                var @operator = ctx.multiplOperator();
                errors.Add(new Error(@operator, $"Illegal operator: operator {@operator.GetText()} cannot be applied to types {leftType} and {rightType}")); // (TODO: check inheritance too)
                return typeSystem["ErrorType"];
            }

            if (context is LogicalExpressionContext)
            {
                var ctx = context as LogicalExpressionContext;
                var leftType = GetType(ctx.left);
                var rightType = GetType(ctx.right);

                if (leftType == rightType && leftType == typeSystem["Boolean"])
                    return typeSystem["Boolean"];

                var @operator = ctx.logicalOperator();
                errors.Add(new Error(@operator, $"Illegal operator: operator {@operator.GetText()} cannot be applied to types {leftType} and {rightType}"));// (TODO: check inheritance too)
                return typeSystem["ErrorType"];
            }


            return null;
        }

    }
}
