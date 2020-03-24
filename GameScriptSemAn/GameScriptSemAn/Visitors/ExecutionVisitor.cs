using Antlr4.Runtime.Misc;
using GameModel.Models;
using GameModel.Models.InstanceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static GameScript.ViGaSParser;

namespace GameScript.Visitors
{
    internal class ExecutionVisitor : ExpressionVisitor
    {
        public ExecutionVisitor(IGameWorldObject gameObject) : base(gameObject)
        {
        }

        public override object VisitVariableDeclaration([NotNull] VariableDeclarationContext context)
        {
            var varName = context.varName().GetText();
            var varType = TypeChecker.GetTypeOfTypeName(context.typeName());

            var expressionValue = context.expression() != null ? Visit(context.expression()) : null;

            if (expressionValue != null && varType != TypeChecker.GetTypeOf(context.expression(), GameObject))
                throw new InvalidTypeException(context.expression(), varType);

            if (GameObject.Variables.TryGetValue(varName, out _))
                throw new VariableAlreadyDeclaredException(varName);

            GameObject.Variables.Add(varName, new Variable()
            {
                Name = varName,
                Type = varType,
                Value = expressionValue.ToString()
            });

            return null;
        }

        public override object VisitStatementList([NotNull] StatementListContext context)
        {
            foreach (var statement in context.children)
            {
                Visit(statement);
            }

            return null;
        }

        public override object VisitIfStatement([NotNull] IfStatementContext context)
        {
            var expression = (bool)Visit(context.expression());

            if (expression)
                Visit(context.statementList());
            else
                if (context.elseStatement() != null)
                    Visit(context.elseStatement().statementList());

            return null;
        }

        public override object VisitAssignmentStatement([NotNull] AssignmentStatementContext context)
        {
            var expressionType = TypeChecker.GetTypeOf(context.expression(), GameObject);
            var expressionValue = Visit(context.expression());
            var variable = (Variable)VisitPath(context.path());

            if (expressionType != variable.Type)
                throw new InvalidTypeException(context.expression(), variable.Type);

            variable.Value = expressionValue.ToString();

            return null;
        }

        public override object VisitFunctionCallStatement([NotNull] FunctionCallStatementContext context)
        {
            var path = context.path();
            var functionName = context.functionName().GetText();
            var parameterList = context.functionParameterList()?.expression().ToList();
            var parameterListTypes = parameterList?.Select(p => TypeChecker.GetTypeOf(p, GameObject)).ToArray();
            var parameterListValues = parameterList?.Select(p => Visit(p)).ToArray();

            object subject = GameObject.World;
            if (path != null)
                subject = VisitPath(path);

            MethodInfo method = subject.GetType().GetMethod(functionName, parameterListTypes ?? new Type[0]);

            return method.Invoke(subject, parameterListValues);
        }

    }
}
