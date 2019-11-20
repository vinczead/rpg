using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace GameScript.Visitors
{
    public class TypeVisitor : GameScriptBaseVisitor<object>
    {
        public override object VisitRefExpression([NotNull] GameScriptParser.RefExpressionContext context)
        {
            return typeof(ReferenceType);
        }

        public override object VisitStringExpression([NotNull] GameScriptParser.StringExpressionContext context)
        {
            return typeof(string);
        }

        public override object VisitBoolExpression([NotNull] GameScriptParser.BoolExpressionContext context)
        {
            return typeof(bool);
        }

        public override object VisitNumberExpression([NotNull] GameScriptParser.NumberExpressionContext context)
        {
            return typeof(double);
        }

        public override object VisitNotExpression([NotNull] GameScriptParser.NotExpressionContext context)
        {
            if ((Type)Visit(context.expression()) == typeof(bool))
                return typeof(bool);
            else
                return typeof(ErrorType);
        }

        public override object VisitCompExpression([NotNull] GameScriptParser.CompExpressionContext context)
        {
            var leftType = (Type)Visit(context.left);
            var rightType = (Type)Visit(context.right);
            var @operator = context.compOperator();

            if (leftType != rightType)
                return typeof(ErrorType);

            //operators other than == and != are only valid on numbers
            if (@operator.EQ() == null && @operator.NEQ() == null && leftType != typeof(double))
                return typeof(ErrorType);

            return typeof(bool);
        }

        public override object VisitAdditiveExpression([NotNull] GameScriptParser.AdditiveExpressionContext context)
        {
            var leftType = (Type)Visit(context.left);
            var rightType = (Type)Visit(context.right);
            var @operator = context.additiveOperator();

            if (leftType == typeof(string) && @operator.PLUS() != null)
                return leftType;

            if (leftType == rightType && leftType == typeof(double))
                return leftType;

            return typeof(ErrorType);
        }

        public override object VisitMultiplExpression([NotNull] GameScriptParser.MultiplExpressionContext context)
        {
            var leftType = (Type)Visit(context.left);
            var rightType = (Type)Visit(context.right);

            if (leftType == rightType && leftType == typeof(double))
                return leftType;

            return typeof(ErrorType);
        }

        public override object VisitLogicalExpression([NotNull] GameScriptParser.LogicalExpressionContext context)
        {
            var leftType = (Type)Visit(context.left);
            var rightType = (Type)Visit(context.right);

            if (leftType == rightType && leftType == typeof(bool))
                return leftType;

            return typeof(ErrorType);
        }

    }
}
