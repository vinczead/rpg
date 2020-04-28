using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using static GameScript.ViGaSParser;

namespace GameScript.Models.Script
{
    public static class TypeChecker
    {

        public static Type GetType(PathContext context)
        {
            return GetType(context, new List<Error>());
        }

        public static Type GetType(ExpressionContext context)
        {
            return GetType(context, new List<Error>());
        }

        public static Type GetType(PathContext context, List<Error> errors)
        {
            /*var variable = GetSymbolFromEnv(context, context.varPath()?.varName()[0].GetText());

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
            return type;*/
            throw new NotImplementedException();
        }

        public static Type GetType(ExpressionContext context, List<Error> errors)
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
                /*var ctx = context as PathExpressionContext;
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
                return type;*/
                throw new NotImplementedException();
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
                var type = GetType((context as NotExpressionContext).expression(), errors);

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
                var leftType = GetType(ctx.left, errors);
                var rightType = GetType(ctx.right, errors);
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
                var leftType = GetType(ctx.left, errors);
                var rightType = GetType(ctx.right, errors);
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
                var leftType = GetType(ctx.left, errors);
                var rightType = GetType(ctx.right, errors);

                if (leftType == rightType && leftType.InheritsFrom(TypeSystem.Instance["Number"]))
                    return TypeSystem.Instance["Number"];

                var @operator = ctx.multiplOperator();
                errors.Add(new Error(@operator, $"Illegal operator: operator {@operator.GetText()} cannot be applied to types {leftType} and {rightType}")); // (TODO: check inheritance too)
                return TypeSystem.Instance["ErrorType"];
            }

            if (context is LogicalExpressionContext)
            {
                var ctx = context as LogicalExpressionContext;
                var leftType = GetType(ctx.left, errors);
                var rightType = GetType(ctx.right, errors);

                if (leftType == rightType && leftType.InheritsFrom(TypeSystem.Instance["Boolean"]))
                    return TypeSystem.Instance["Boolean"];

                var @operator = ctx.logicalOperator();
                errors.Add(new Error(@operator, $"Illegal operator: operator {@operator.GetText()} cannot be applied to types {leftType} and {rightType}"));// (TODO: check inheritance too)
                return TypeSystem.Instance["ErrorType"];
            }

            if (context is ParenExpressionContext)
                return GetType((context as ParenExpressionContext).expression(), errors);

            if (context is ArrayExpressionContext)
            {
                var ctx = context as ArrayExpressionContext;
                Models.Script.Type firstType;

                if (ctx.expression() == null || (firstType = GetType(ctx.expression()[0], errors)) == TypeSystem.Instance["ErrorType"])
                {
                    errors.Add(new Error(ctx, $"Invalid array expression."));
                    return TypeSystem.Instance["ErrorType"];
                }

                foreach (var expr in ctx.expression())
                {
                    if (GetType(expr, errors) != firstType)
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
