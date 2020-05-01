﻿using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using GameScript.Models;
using GameScript.Models.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Visitors
{
    public sealed class TypeVisitor : ViGaSBaseVisitor<Models.Script.Type>
    {
        Env env;
        List<Error> errors;

        public static TypeVisitor Instance { get; } = new TypeVisitor();

        private TypeVisitor()
        {
            env = new Env();
            errors = new List<Error>();
        }

        public static Models.Script.Type GetType(IParseTree parseTree, Env env, List<Error> errors)
        {
            Instance.env = env;
            Instance.errors = errors;

            return Instance.Visit(parseTree);
        }

        public override Models.Script.Type VisitPath([NotNull] ViGaSParser.PathContext context)
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

        public override Models.Script.Type VisitArrayExpression([NotNull] ViGaSParser.ArrayExpressionContext context)
        {
            if (context.expression() == null)
            {
                errors.Add(new Error(context, $"Invalid array expression."));
                return TypeSystem.Instance["ErrorType"];
            }

            var firstType = Visit(context.expression()[0]);

            foreach (var expr in context.expression())
            {
                if (Visit(expr) != firstType)
                {
                    errors.Add(new Error(expr, $"Invalid array expression: arrays can only contain elements of the same type."));
                    return TypeSystem.Instance["ErrorType"];
                }
            }
            return TypeSystem.Instance[$"{firstType}Array"];
        }

        public override Models.Script.Type VisitBoolExpression([NotNull] ViGaSParser.BoolExpressionContext context)
        {
            return TypeSystem.Instance["Boolean"];
        }

        public override Models.Script.Type VisitNumberExpression([NotNull] ViGaSParser.NumberExpressionContext context)
        {
            return TypeSystem.Instance["Number"];
        }

        public override Models.Script.Type VisitStringExpression([NotNull] ViGaSParser.StringExpressionContext context)
        {
            return TypeSystem.Instance["String"];
        }

        public override Models.Script.Type VisitNullExpression([NotNull] ViGaSParser.NullExpressionContext context)
        {
            return TypeSystem.Instance["NullType"];
        }

        public override Models.Script.Type VisitNotExpression([NotNull] ViGaSParser.NotExpressionContext context)
        {
            var type = Visit(context.expression());

            if (!type.InheritsFrom(TypeSystem.Instance["Boolean"]))
            {
                errors.Add(new Error(context, $"Type mismatch: expression type must be {TypeSystem.Instance["Boolean"]}"));
                return TypeSystem.Instance["ErrorType"];
            }

            return type;
        }

        public override Models.Script.Type VisitParenExpression([NotNull] ViGaSParser.ParenExpressionContext context)
        {
            return Visit(context.expression());
        }

        public override Models.Script.Type VisitPathExpression([NotNull] ViGaSParser.PathExpressionContext context)
        {
            return base.Visit(context.path());
        }

        public override Models.Script.Type VisitRefExpression([NotNull] ViGaSParser.RefExpressionContext context)
        {
            //Get Id from GameModel
            throw new NotImplementedException();
        }

        public override Models.Script.Type VisitFuncExpression([NotNull] ViGaSParser.FuncExpressionContext context)
        {
            var funcNameCtx = context.functionCallStatement().functionName();

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

        //TODO: leftType == rightType should be changed to leftType.InheritsFrom(rightType) || rightType.InheritsFrom(leftType)

        public override Models.Script.Type VisitAdditiveExpression([NotNull] ViGaSParser.AdditiveExpressionContext context)
        {
            var leftType = Visit(context.left);
            var rightType = Visit(context.right);
            var op = context.additiveOperator();

            if (leftType.InheritsFrom(TypeSystem.Instance["String"]) && op.PLUS() != null)
                return TypeSystem.Instance["String"];

            if (leftType == rightType && leftType.InheritsFrom(TypeSystem.Instance["Number"]))
                return TypeSystem.Instance["Number"];

            errors.Add(new Error(op, $"Illegal operator: operator {op.GetText()} cannot be applied to types {leftType} and {rightType}"));
            return TypeSystem.Instance["ErrorType"];
        }

        public override Models.Script.Type VisitMultiplExpression([NotNull] ViGaSParser.MultiplExpressionContext context)
        {
            var leftType = Visit(context.left);
            var rightType = Visit(context.right);

            if (leftType == rightType && leftType.InheritsFrom(TypeSystem.Instance["Number"]))
                return TypeSystem.Instance["Number"];

            var op = context.multiplOperator();
            errors.Add(new Error(op, $"Illegal operator: operator {op.GetText()} cannot be applied to types {leftType} and {rightType}"));
            return TypeSystem.Instance["ErrorType"];
        }

        public override Models.Script.Type VisitLogicalExpression([NotNull] ViGaSParser.LogicalExpressionContext context)
        {
            var leftType = Visit(context.left);
            var rightType = Visit(context.right);

            if (leftType == rightType && leftType.InheritsFrom(TypeSystem.Instance["Boolean"]))
                return TypeSystem.Instance["Boolean"];

            var op = context.logicalOperator();
            errors.Add(new Error(op, $"Illegal operator: operator {op.GetText()} cannot be applied to types {leftType} and {rightType}"));// (TODO: check inheritance too)
            return TypeSystem.Instance["ErrorType"];
        }

        public override Models.Script.Type VisitCompExpression([NotNull] ViGaSParser.CompExpressionContext context)
        {
            var leftType = Visit(context.left);
            var rightType = Visit(context.right);

            var op = context.compOperator();

            if (!leftType.InheritsFrom(rightType) && !rightType.InheritsFrom(leftType))
            {
                errors.Add(new Error(context, $"Illegal operator: operator {op.GetText()} cannot be applied to types {leftType} and {rightType}"));
                return TypeSystem.Instance["ErrorType"];
            }

            //operators <, >, <=, >= are only defined on numbers
            if (op.EQ() == null && op.NEQ() == null && !leftType.InheritsFrom(TypeSystem.Instance["Number"]))
            {
                errors.Add(new Error(context, $"Illegal operator: operator {op.GetText()} cannot be applied to types {leftType} and {rightType}"));
                return TypeSystem.Instance["ErrorType"];
            }

            return TypeSystem.Instance["Boolean"];
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
    }
}