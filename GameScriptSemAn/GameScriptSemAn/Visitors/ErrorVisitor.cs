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
        public List<Error> errors;

        public override object VisitScript([NotNull] ScriptContext context)
        {
            env = new Env();
            errors = new List<Error>();

            return base.VisitScript(context);
        }

        public override object VisitBaseDefinition([NotNull] BaseDefinitionContext context)
        {
            var baseClass = context.baseHeader().baseClass();
            var baseId = context.baseHeader().baseId();

            var baseClassType = TypeSystem.Instance["ErrorType"];
            if (baseClass != null)
            {
                try
                {
                    baseClassType = TypeSystem.Instance[baseClass.GetText()];
                }
                catch (KeyNotFoundException e)
                {
                    errors.Add(new Error(baseClass, e.Message));
                }
                if (!baseClassType.InheritsFrom(TypeSystem.Instance["GameObject"]))
                    errors.Add(new Error(baseClass, $"Type {baseClassType} is not valid here (must inherit from {TypeSystem.Instance["GameObject"]})."));
            }
            //TODO: add newly defined type {baseId.GetText()} to type system.

            env = new Env(env, baseId.GetText());
            AddSymbolToEnv(context, new Symbol("Base", baseClassType));

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
            var eventCtx = context.eventTypeName();

            var baseType = GetSymbolFromEnv(context, "Base").Type;

            var @event = baseType.Events.FirstOrDefault(e => e.Name == eventCtx.GetText());

            if (@event == null)
            {
                errors.Add(new Error(eventCtx, $"Event {eventCtx.GetText()} is not defined on {baseType}."));
                return base.VisitRunBlock(context);
            }
            else
            {
                env = new Env(env, eventCtx.GetText());
                AddSymbolToEnv(context, new Symbol("Self", TypeSystem.Instance[$"{baseType}Instance"]));

                foreach (var param in @event.Parameters)
                    AddSymbolToEnv(eventCtx, new Symbol(param.Name, param.Type));

                var result = base.VisitRunBlock(context);

                env = env.Previous;
                return result;
            }
        }

        public override object VisitIfStatement([NotNull] IfStatementContext context)
        {
            var type = GetType(context.expression());

            if (!type.InheritsFrom(TypeSystem.Instance["Boolean"]))
                errors.Add(new Error(context, $"Expression type must be {TypeSystem.Instance["Boolean"]}"));

            return base.VisitIfStatement(context);
        }

        public override object VisitRepeatStatement([NotNull] RepeatStatementContext context)
        {
            var type = GetType(context.expression());

            if (!type.InheritsFrom(TypeSystem.Instance["Boolean"]))
                errors.Add(new Error(context, $"Expression type must be {TypeSystem.Instance["Boolean"]}"));

            return base.VisitRepeatStatement(context);
        }

        public override object VisitWhileStatement([NotNull] WhileStatementContext context)
        {
            var type = GetType(context.expression());

            if (!type.InheritsFrom(TypeSystem.Instance["Boolean"]))
                errors.Add(new Error(context, $"Expression type must be {TypeSystem.Instance["Boolean"]}"));

            return base.VisitWhileStatement(context);
        }

        public override object VisitVariableDeclaration([NotNull] VariableDeclarationContext context)
        {
            //check whether expression after WithValue matches type
            var varName = context.varName()?.GetText();

            Model.Type varType = TypeSystem.Instance["ErrorType"];

            try
            {
                varType = TypeSystem.Instance[context.typeName()?.GetText()];
            }
            catch (KeyNotFoundException e)
            {
                errors.Add(new Error(context.typeName(), e.Message));
            }

            AddSymbolToEnv(context, new Symbol(varName, varType));

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
            var expressionType = GetType(context.expression()) ?? TypeSystem.Instance["ErrorType"];
            var symbolType = GetType(context.path());

            if (symbolType != TypeSystem.Instance["ErrorType"] && !expressionType.InheritsFrom(symbolType))
            {
                errors.Add(new Error(context.expression(), $"Type mismatch: expression of type {expressionType} cannot be assigned to a variable of type {symbolType}"));
            }
            return base.VisitAssignmentStatement(context);
        }

        public override object VisitFunctionCallStatement([NotNull] FunctionCallStatementContext context)
        {
            //check whether function exists and parameter types match
            var funcNameCtx = context.functionName();

            try
            {
                var function = FunctionManager.Instance[funcNameCtx.GetText()];

                var paramsCtx = context.functionParameterList();
                var requiredParamCount = function.Parameters.Count;
                var actualParamCount = paramsCtx?.expression()?.Length ?? 0;

                if (actualParamCount == requiredParamCount)
                {
                    for (int i = 0; i < function.Parameters.Count; i++)
                    {
                        if (!GetType(paramsCtx.expression(i)).InheritsFrom(function.Parameters[i].Type))
                            errors.Add(new Error(funcNameCtx, $"Type mismatch: The function {function} expects {function.Parameters[i].Type} as parameter {i + 1}, not {GetType(paramsCtx.expression(i))}."));
                    }
                }
                else
                    errors.Add(new Error(funcNameCtx, $"The function {function} requires {requiredParamCount} parameter(s), not {actualParamCount}."));
            }
            catch (KeyNotFoundException e)
            {
                errors.Add(new Error(funcNameCtx, e.Message));
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
            if (symbolName == null)
                return null;

            var symbol = env[symbolName];
            if (symbol != null)
                return symbol;

            errors.Add(new Error(context, $"{symbolName} does not exist in this context."));
            return null;
        }

        private Model.Type GetType(PathContext context)
        {
            var variable = GetSymbolFromEnv(context, context.varPath()?.varName()[0].GetText());

            if (variable == null)
                return TypeSystem.Instance["ErrorType"];

            var type = variable.Type;

            foreach (var varName in context.varPath().varName().Skip(1).ToArray())
            {
                var prop = type.Properties.FirstOrDefault(p => p.Name == varName.GetText());
                if (prop == null)
                {
                    errors.Add(new Error(context, $"{varName.GetText()} is not defined on {type}."));
                    return TypeSystem.Instance["ErrorType"];
                }

                type = prop.Type;
            }
            return type;
        }

        private Model.Type GetType(ExpressionContext context)
        {
            if (context is FuncExpressionContext)
            {
                var ctx = (context as FuncExpressionContext).functionCallStatement();
                //check whether function exists and parameter types match
                var funcNameCtx = ctx.functionName();

                try
                {
                    var function = FunctionManager.Instance[funcNameCtx.GetText()];
                    return function.ReturnType;
                }
                catch (KeyNotFoundException)
                {
                    return TypeSystem.Instance["ErrorType"];
                }
            }

            if (context is PathExpressionContext)
            {
                var ctx = context as PathExpressionContext;
                var variable = GetSymbolFromEnv(ctx, ctx.path().varPath().varName()[0].GetText());

                if (variable == null)
                    return TypeSystem.Instance["ErrorType"];

                var type = variable.Type;

                foreach (var varName in ctx.path().varPath().varName().Skip(1).ToArray())
                {
                    var prop = type.Properties.FirstOrDefault(p => p.Name == varName.GetText());
                    if (prop == null)
                    {
                        errors.Add(new Error(context, $"{varName.GetText()} is not defined on {type}."));
                        return TypeSystem.Instance["ErrorType"];
                    }

                    type = prop.Type;
                }
                return type;
            }

            if (context is StringExpressionContext)
                return TypeSystem.Instance["String"];

            if (context is RefExpressionContext)
                throw new NotImplementedException();

            if (context is BoolExpressionContext)
                return TypeSystem.Instance["Boolean"];

            if (context is NumberExpressionContext)
                return TypeSystem.Instance["Number"];

            if (context is NullExpressionContext)
                return TypeSystem.Instance["NullType"];

            if (context is NotExpressionContext)
            {
                var type = GetType((context as NotExpressionContext).expression());

                if (!type.InheritsFrom(TypeSystem.Instance["Boolean"]))
                {
                    errors.Add(new Error(context, $"Type mismatch: expression type must be {TypeSystem.Instance["Boolean"]}"));
                    return TypeSystem.Instance["ErrorType"];
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
                    return TypeSystem.Instance["ErrorType"];
                }

                //operators <, >, <=, >= are only defined on numbers
                if (@operator.EQ() == null && @operator.NEQ() == null && !leftType.InheritsFrom(TypeSystem.Instance["Number"]))
                {
                    errors.Add(new Error(ctx, $"Illegal operator: operator {@operator.GetText()} cannot be applied to types {leftType} and {rightType}"));
                    return TypeSystem.Instance["ErrorType"];
                }

                return TypeSystem.Instance["Boolean"];
            }

            if (context is AdditiveExpressionContext)
            {
                var ctx = context as AdditiveExpressionContext;
                var leftType = GetType(ctx.left);
                var rightType = GetType(ctx.right);
                var @operator = ctx.additiveOperator();

                if (leftType.InheritsFrom(TypeSystem.Instance["String"]) && @operator.PLUS() != null)
                    return TypeSystem.Instance["String"];

                if (leftType == rightType && leftType.InheritsFrom(TypeSystem.Instance["Number"]))
                    return TypeSystem.Instance["Number"];

                errors.Add(new Error(@operator, $"Illegal operator: operator {@operator.GetText()} cannot be applied to types {leftType} and {rightType}"));// (TODO: check inheritance too)
                return TypeSystem.Instance["ErrorType"];
            }

            if (context is MultiplExpressionContext)
            {
                var ctx = context as MultiplExpressionContext;
                var leftType = GetType(ctx.left);
                var rightType = GetType(ctx.right);

                if (leftType == rightType && leftType.InheritsFrom(TypeSystem.Instance["Number"]))
                    return TypeSystem.Instance["Number"];

                var @operator = ctx.multiplOperator();
                errors.Add(new Error(@operator, $"Illegal operator: operator {@operator.GetText()} cannot be applied to types {leftType} and {rightType}")); // (TODO: check inheritance too)
                return TypeSystem.Instance["ErrorType"];
            }

            if (context is LogicalExpressionContext)
            {
                var ctx = context as LogicalExpressionContext;
                var leftType = GetType(ctx.left);
                var rightType = GetType(ctx.right);

                if (leftType == rightType && leftType.InheritsFrom(TypeSystem.Instance["Boolean"]))
                    return TypeSystem.Instance["Boolean"];

                var @operator = ctx.logicalOperator();
                errors.Add(new Error(@operator, $"Illegal operator: operator {@operator.GetText()} cannot be applied to types {leftType} and {rightType}"));// (TODO: check inheritance too)
                return TypeSystem.Instance["ErrorType"];
            }

            if (context is ParenExpressionContext)
                return GetType((context as ParenExpressionContext).expression());

            if (context is ArrayExpressionContext)
            {
                var ctx = context as ArrayExpressionContext;
                Model.Type firstType;

                if (ctx.expression() == null || (firstType = GetType(ctx.expression()[0])) == TypeSystem.Instance["ErrorType"])
                {
                    errors.Add(new Error(ctx, $"Invalid array expression."));
                    return TypeSystem.Instance["ErrorType"];
                }

                foreach (var expr in ctx.expression())
                {
                    if(GetType(expr) != firstType)
                    {
                        errors.Add(new Error(expr, $"Invalid array expression: arrays can only contain elements of the same type."));
                        return TypeSystem.Instance["ErrorType"];
                    }
                }
                return TypeSystem.Instance[$"{firstType}Array"];

            }

            return TypeSystem.Instance["ErrorType"];
        }

    }
}
