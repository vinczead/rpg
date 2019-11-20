using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using static GameScript.GameScriptParser;

namespace GameScript.Visitors
{
    public static class TypeChecker
    {
        public static Type GetTypeOfFunctionCallStatement([NotNull] FunctionCallStatementContext context)
        {
            return typeof(string);
        }

        public static Type GetTypeOfTypeName([NotNull] TypeNameContext context)
        {
            switch (context.GetText())
            {
                case "Boolean":
                    return typeof(bool);
                case "Number":
                    return typeof(double);
                case "String":
                    return typeof(string);
                case "Reference":
                    return typeof(ReferenceType);
                default:
                    return typeof(ErrorType);
            }
        }

        public static Type GetTypeOfRefExpression([NotNull] RefExpressionContext context)
        {
            return typeof(ReferenceType);
        }

        public static Type GetTypeOfStringExpression([NotNull] StringExpressionContext context)
        {
            return typeof(string);
        }

        public static Type GetTypeOfBoolExpression([NotNull] BoolExpressionContext context)
        {
            return typeof(bool);
        }

        public static Type GetTypeOfNumberExpression([NotNull] NumberExpressionContext context)
        {
            return typeof(double);
        }

        public static Type GetTypeOfNotExpression([NotNull] NotExpressionContext context)
        {
            if (GetTypeOf(context.expression()) == typeof(bool))
                return typeof(bool);
            else
                return typeof(ErrorType);
        }

        public static Type GetTypeOfCompExpression([NotNull] CompExpressionContext context)
        {
            var leftType = GetTypeOf(context.left);
            var rightType = GetTypeOf(context.right);
            var @operator = context.compOperator();

            if (leftType != rightType)
                return typeof(ErrorType);

            //operators other than == and != are only valid on numbers
            if (@operator.EQ() == null && @operator.NEQ() == null && leftType != typeof(double))
                return typeof(ErrorType);

            return typeof(bool);
        }

        public static Type GetTypeOfAdditiveExpression([NotNull] AdditiveExpressionContext context)
        {
            var leftType = GetTypeOf(context.left);
            var rightType = GetTypeOf(context.right);
            var @operator = context.additiveOperator();

            if (leftType == typeof(string) && @operator.PLUS() != null)
                return leftType;

            if (leftType == rightType && leftType == typeof(double))
                return leftType;

            return typeof(ErrorType);
        }

        public static Type GetTypeOfMultiplExpression([NotNull] MultiplExpressionContext context)
        {
            var leftType = GetTypeOf(context.left);
            var rightType = GetTypeOf(context.right);

            if (leftType == rightType && leftType == typeof(double))
                return leftType;

            return typeof(ErrorType);
        }

        public static Type GetTypeOfLogicalExpression([NotNull] LogicalExpressionContext context)
        {
            var leftType = GetTypeOf(context.left);
            var rightType = GetTypeOf(context.right);

            if (leftType == rightType && leftType == typeof(bool))
                return leftType;

            return typeof(ErrorType);
        }

        public static Type GetTypeOf([NotNull]ExpressionContext context)
        {
            if (context is PathExpressionContext)
                return typeof(string);

            if (context is RefExpressionContext)
                return GetTypeOfRefExpression(context as RefExpressionContext);

            if (context is StringExpressionContext)
                return GetTypeOfStringExpression(context as StringExpressionContext);

            if (context is BoolExpressionContext)
                return GetTypeOfBoolExpression(context as BoolExpressionContext);

            if (context is NumberExpressionContext)
                return GetTypeOfNumberExpression(context as NumberExpressionContext);

            if (context is NotExpressionContext)
                return GetTypeOfNotExpression(context as NotExpressionContext);

            if (context is CompExpressionContext)
                return GetTypeOfCompExpression(context as CompExpressionContext);

            if (context is AdditiveExpressionContext)
                return GetTypeOfAdditiveExpression(context as AdditiveExpressionContext);

            if (context is MultiplExpressionContext)
                return GetTypeOfMultiplExpression(context as MultiplExpressionContext);

            if (context is LogicalExpressionContext)
                return GetTypeOfLogicalExpression(context as LogicalExpressionContext);

            if (context is FuncExpressionContext)
                return GetTypeOfFunctionCallStatement((context as FuncExpressionContext).functionCallStatement());

            throw new ArgumentException("Invalid parameter", "context");

        }

    }
}
