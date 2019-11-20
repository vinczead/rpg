using Antlr4.Runtime.Misc;
using GameModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameScript.Visitors
{
    internal class ExecutionVisitor : ExpressionVisitor
    {
        public ExecutionVisitor(World world, GameObject gameObject) : base(world, gameObject)
        {
        }

        public override object VisitVariableDeclaration([NotNull] GameScriptParser.VariableDeclarationContext context)
        {
            var varName = context.varName().GetText();
            var varType = TypeChecker.GetTypeOfTypeName(context.typeName());

            var expressionValue = context.expression() != null ? Visit(context.expression()) : null;

            if (expressionValue != null && varType != TypeChecker.GetTypeOf(context.expression()))
                throw new InvalidTypeException(context.expression(), varType);

            //todo add check of duplicate variables

            GameObject.Variables[varName].Value = expressionValue.ToString();

            return null;
        }

        public override object VisitStatementList([NotNull] GameScriptParser.StatementListContext context)
        {
            foreach (var statement in context.children)
            {
                Visit(statement);
            }

            return null;
        }

        public override object VisitIfStatement([NotNull] GameScriptParser.IfStatementContext context)
        {
            var expression = (bool)Visit(context.expression());

            if (expression)
            {
                Visit(context.statementList());
            }
            else
            {
                if (context.elseStatement() != null)
                    Visit(context.elseStatement().statementList());
            }

            return null;
        }

        public override object VisitAssignmentStatement([NotNull] GameScriptParser.AssignmentStatementContext context)
        {
            var expressionType = TypeChecker.GetTypeOf(context.expression());
            var expressionValue = Visit(context.expression());
            var variable = (Variable)VisitPath(context.path());

            if (expressionType != variable.Type)
                throw new InvalidTypeException(context.expression(), variable.Type);

            variable.Value = expressionValue.ToString();

            return null;
        }

        

        public override object VisitFunctionCallStatement([NotNull] GameScriptParser.FunctionCallStatementContext context)
        {
            //var path = context.path();
            var functionName = context.functionName().GetText();
            var parameterList = context.functionParameterList().expression().ToList();
            var parameterListTypes = parameterList.Select(p => TypeChecker.GetTypeOf(p)).ToArray();
            var parameterListValues = parameterList.Select(p => Visit(p)).ToArray();

            var subject = World;

            MethodInfo method = subject.GetType().GetMethod(functionName, parameterListTypes);

            return method.Invoke(World, parameterListValues);
        }

    }
}
