﻿using Antlr4.Runtime.Misc;
using GameModel.Models;
using GameScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Visitors
{
    public class ExpressionVisitor : GameScriptBaseVisitor<object>
    {
        public World World { get; set; }
        public GameObject GameObject { get; set; }

        public ExpressionVisitor(World world, GameObject gameObject)
        {
            World = world;
            GameObject = gameObject;
        }

        public override object VisitPath([NotNull] GameScriptParser.PathContext context)
        {
            var varPath = context.varPath();
            var refPath = context.refPath();

            if (varPath != null)
            {
                return (Variable)VisitVarPath(varPath);
            }

            if (refPath != null)
            {
                return (Variable)VisitRefPath(refPath);
            }

            return null;
        }

        public override object VisitPathExpression([NotNull] GameScriptParser.PathExpressionContext context)
        {
            return (VisitPath(context.path()) as Variable)?.Value;
        }

        public override object VisitVarPath([NotNull] GameScriptParser.VarPathContext context)
        {
            Variable variable = null;
            GameObject currentObject = GameObject;

            foreach (var varName in context.varName())
            {
                if (currentObject == null)
                    throw new IdNotFoundException(varName.GetText());

                if (currentObject.Variables.TryGetValue(varName.GetText(), out variable))
                {
                    if (variable.Type == typeof(ReferenceType))
                        currentObject = (GameObject)World.GetById(variable.Name);
                    else
                        currentObject = null;
                }
                else
                    throw new IdNotFoundException(varName.GetText());
            }

            return variable;
        }

        public override object VisitRefPath([NotNull] GameScriptParser.RefPathContext context)
        {
            //todo implement
            return new Variable()
            {
                Name = "a",
                Value = "aa",
                Type = typeof(string)
            };
        }

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
            var value = context.STRING().GetText();
            return value.Substring(1, value.Length - 2);
        }

        public override object VisitNotExpression([NotNull] GameScriptParser.NotExpressionContext context)
        {
            var type = TypeChecker.GetTypeOf(context.expression());

            if (type != typeof(bool))
                throw new InvalidTypeException(context.expression(), typeof(bool));

            return !AsBool(context.expression());

        }

        public override object VisitParenExpression([NotNull] GameScriptParser.ParenExpressionContext context)
        {
            return Visit(context.expression());
        }

        public override object VisitAdditiveExpression([NotNull] GameScriptParser.AdditiveExpressionContext context)
        {
            var left = context.left;
            var right = context.right;
            var leftType = TypeChecker.GetTypeOf(left);
            var rightType = TypeChecker.GetTypeOf(right);

            var @operator = context.additiveOperator();

            if (leftType == typeof(string) && @operator.PLUS() != null)
            {
                if (rightType == typeof(string))
                    return AsString(left) + AsString(right);
                if (rightType == typeof(double))
                    return AsString(left) + AsNumber(right).ToString();
                if (rightType == typeof(bool))
                    return AsString(left) + AsBool(right).ToString();
            }

            if (leftType != typeof(double) || rightType != typeof(double))
                throw new InvalidTypeException(context, typeof(double));

            if (context.additiveOperator().PLUS() != null)
                return AsNumber(context.left) + AsNumber(context.right);

            if (context.additiveOperator().MINUS() != null)
                return AsNumber(context.left) - AsNumber(context.right);

            throw new InvalidOperationException();
        }

        public override object VisitMultiplExpression([NotNull] GameScriptParser.MultiplExpressionContext context)
        {
            var left = context.left;
            var right = context.right;
            var leftType = TypeChecker.GetTypeOf(left);
            var rightType = TypeChecker.GetTypeOf(right);

            if (leftType != typeof(double) || rightType != typeof(double))
                throw new InvalidTypeException(context, typeof(double));

            if (context.multiplOperator().MULT() != null)
                return AsNumber(left) * AsNumber(right);

            if (context.multiplOperator().DIV() != null)
                return AsNumber(left) / AsNumber(right);

            throw new InvalidOperationException();
        }

        public override object VisitCompExpression([NotNull] GameScriptParser.CompExpressionContext context)
        {
            var left = context.left;
            var right = context.right;
            var leftType = TypeChecker.GetTypeOf(left);
            var rightType = TypeChecker.GetTypeOf(right);

            if (leftType != rightType)
                throw new InvalidTypeException(right, leftType);

            if (context.compOperator().EQ() != null)
            {
                if (leftType == typeof(string))
                    return AsString(left) == AsString(right);

                if (leftType == typeof(double))
                    return Math.Abs(AsNumber(left) - AsNumber(right)) < 0.0001;

                if (leftType == typeof(bool))
                    return AsBool(left) == AsBool(right);
            }

            if (context.compOperator().NEQ() != null)
            {
                if (leftType == typeof(string))
                    return AsString(left) != AsString(right);

                if (leftType == typeof(double))
                    return Math.Abs(AsNumber(left) - AsNumber(right)) > 0.0001;

                if (leftType == typeof(bool))
                    return AsBool(left) != AsBool(right);
            }

            if (leftType != typeof(double) || rightType != typeof(double))
                throw new InvalidTypeException(context, typeof(double));

            if (context.compOperator().LT() != null)
                return AsNumber(left) < AsNumber(right);

            if (context.compOperator().GT() != null)
                return AsNumber(left) > AsNumber(right);

            if (context.compOperator().LTE() != null)
                return AsNumber(left) <= AsNumber(right);

            if (context.compOperator().GTE() != null)
                return AsNumber(left) >= AsNumber(right);

            throw new InvalidOperationException();
        }

        public override object VisitLogicalExpression([NotNull] GameScriptParser.LogicalExpressionContext context)
        {
            var left = context.left;
            var right = context.right;
            var leftType = TypeChecker.GetTypeOf(left);
            var rightType = TypeChecker.GetTypeOf(right);

            if (leftType != typeof(bool) || rightType != typeof(bool))
                throw new InvalidTypeException(context, typeof(bool));

            if (context.logicalOperator().AND() != null)
                return AsBool(left) & AsBool(right);

            if (context.logicalOperator().OR() != null)
                return AsBool(left) | AsBool(right);

            if (context.logicalOperator().XOR() != null)
                return AsBool(left) ^ AsBool(right);

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

        private string AsString(GameScriptParser.ExpressionContext context)
        {
            return (string)Visit(context);
        }
    }
}
