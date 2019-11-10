using Antlr4.Runtime.Misc;
using Rpg.Models;
using System;

namespace GameScriptSemAn.Execution
{
    internal class GameScriptExecutionVisitor : GameScriptBaseVisitor<object>
    {
        public World World { get; set; }
        public GameObject GameObject { get; set; }

        public override object VisitNumberExpression([NotNull] GameScriptParser.NumberExpressionContext context)
        {
            return double.Parse(context.NUMBER().GetText());
        }

        public override object VisitBoolExpression([NotNull] GameScriptParser.BoolExpressionContext context)
        {
            return bool.Parse(context.BOOLEAN().GetText());
        }

        public override object VisitStringExpression([NotNull] GameScriptParser.StringExpressionContext context)
        {
            return context.STRING().GetText();
        }

        public override object VisitNotExpression([NotNull] GameScriptParser.NotExpressionContext context)
        {
            return !AsBool(context.expression());
        }

        public override object VisitParenExpression([NotNull] GameScriptParser.ParenExpressionContext context)
        {
            return Visit(context.expression());
        }

        public override object VisitAdditiveExpression([NotNull] GameScriptParser.AdditiveExpressionContext context)
        {
            //todo: implement PLUS for string+string, string+int, string+boolean
            if(context.additiveOperator().PLUS() != null)
                return AsNumber(context.left) + AsNumber(context.right);

            if (context.additiveOperator().MINUS() != null)
                return AsNumber(context.left) - AsNumber(context.right);

            throw new InvalidOperationException();
        }
        
        public override object VisitMultiplExpression([NotNull] GameScriptParser.MultiplExpressionContext context)
        {
            if (context.multiplOperator().MULT() != null)
                return AsNumber(context.left) * AsNumber(context.right);

            if (context.multiplOperator().DIV() != null)
                return AsNumber(context.left) / AsNumber(context.right);

            throw new InvalidOperationException();
        }

        public override object VisitCompExpression([NotNull] GameScriptParser.CompExpressionContext context)
        {
            if (context.compOperator().LT() != null)
                return AsNumber(context.left) < AsNumber(context.right);

            if (context.compOperator().GT() != null)
                return AsNumber(context.left) > AsNumber(context.right);

            if (context.compOperator().LTE() != null)
                return AsNumber(context.left) <= AsNumber(context.right);

            if (context.compOperator().GTE() != null)
                return AsNumber(context.left) >= AsNumber(context.right);

            //TODO: implement EQ, NEQ for string and boolean
            if (context.compOperator().EQ() != null)
                return AsNumber(context.left) == AsNumber(context.right);

            if (context.compOperator().NEQ() != null)
                return AsNumber(context.left) != AsNumber(context.right);

            throw new InvalidOperationException();
        }

        public override object VisitLogicalExpression([NotNull] GameScriptParser.LogicalExpressionContext context)
        {
            if (context.logicalOperator().AND() != null)
                return AsBool(context.left) & AsBool(context.right);

            if (context.logicalOperator().OR() != null)
                return AsBool(context.left) | AsBool(context.right);

            if (context.logicalOperator().XOR() != null)
                return AsBool(context.left) ^ AsBool(context.right);

            return new InvalidOperationException();
        }

        private double AsNumber(GameScriptParser.ExpressionContext context)
        {
            return (double)Visit(context);
        }

        private bool AsBool(GameScriptParser.ExpressionContext context)
        {
            return (bool)Visit(context);
        }

        public override object VisitAssignmentStatement([NotNull] GameScriptParser.AssignmentStatementContext context)
        {
            var x = Visit(context.expression());
            Console.WriteLine(x);
            return x;
        }
    }
}
