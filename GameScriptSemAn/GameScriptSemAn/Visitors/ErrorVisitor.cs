using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using GameScript.Models.Script;
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
            var baseClass = context.baseClass();
            var baseId = context.baseId();

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
            AddSymbolToEnv(context, new Symbol("_Base", baseClassType));


            env = new Env(env, $"{baseId.GetText()} Initialization");
            AddSymbolToEnv(context, new Symbol("Self", baseClassType));

            foreach (var prop in baseClassType.Properties)
                AddSymbolToEnv(baseClass, new Symbol(prop.Name, prop.Type));

            var result = base.VisitBaseDefinition(context);

            env = env.Previous;

            return result;
        }

        public override object VisitInitBlock([NotNull] InitBlockContext context)
        {
            var retVal = base.VisitInitBlock(context);

            env = env.Previous;
                 
            return retVal;
        }

        public override object VisitInstanceDefinition([NotNull] InstanceDefinitionContext context)
        {
            var baseId = context.baseId();
            var instanceId = context.instanceId();

            //TODO: check whether {baseId.GetText()} is a defined base type.
            //store instanceId, if set, so others can reference it

            env = new Env(env, instanceId?.GetText() ?? $"Instance of {baseId.GetText()}");
            var result = base.VisitInstanceDefinition(context);

            env = env.Previous;

            return result;
        }

        public override object VisitRegionDefinition([NotNull] RegionDefinitionContext context)
        {
            //todo: configure ENV
            return base.VisitRegionDefinition(context);
        }

        public override object VisitRunBlock([NotNull] RunBlockContext context)
        {
            var eventCtx = context.eventTypeName();

            var baseType = GetSymbolFromEnv(context, "_Base").Type;

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
            var type = new TypeVisitor(env, errors).Visit(context.expression());

            if (!type.InheritsFrom(TypeSystem.Instance["Boolean"]))
                errors.Add(new Error(context, $"Expression type must be {TypeSystem.Instance["Boolean"]}"));

            return base.VisitIfStatement(context);
        }

        public override object VisitRepeatStatement([NotNull] RepeatStatementContext context)
        {
            var type = new TypeVisitor(env, errors).Visit(context.expression());

            if (!type.InheritsFrom(TypeSystem.Instance["Boolean"]))
                errors.Add(new Error(context, $"Expression type must be {TypeSystem.Instance["Boolean"]}"));

            return base.VisitRepeatStatement(context);
        }

        public override object VisitWhileStatement([NotNull] WhileStatementContext context)
        {
            var type = new TypeVisitor(env, errors).Visit(context.expression());

            if (!type.InheritsFrom(TypeSystem.Instance["Boolean"]))
                errors.Add(new Error(context, $"Expression type must be {TypeSystem.Instance["Boolean"]}"));

            return base.VisitWhileStatement(context);
        }

        public override object VisitVariableDeclaration([NotNull] VariableDeclarationContext context)
        {
            //check whether expression after WithValue matches type
            var varName = context.varName()?.GetText();

            Models.Script.Type varType = TypeSystem.Instance["ErrorType"];

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
                var expressionType = new TypeVisitor(env, errors).Visit(context.expression());

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
            var expressionType = new TypeVisitor(env, errors).Visit(context.expression()) ?? TypeSystem.Instance["ErrorType"];
            var symbolType = new TypeVisitor(env, errors).Visit(context.path());

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
                        var paramType = new TypeVisitor(env, errors).Visit(paramsCtx.expression(i));
                        if (!paramType.InheritsFrom(function.Parameters[i].Type))
                            errors.Add(new Error(funcNameCtx, $"Type mismatch: The function {function} expects {function.Parameters[i].Type} as parameter {i + 1}, not {paramType}."));
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
    }
}
