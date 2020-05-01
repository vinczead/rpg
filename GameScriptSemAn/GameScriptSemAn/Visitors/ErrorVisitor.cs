using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using GameScript.Models.Script;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using static GameScript.ViGaSParser;

namespace GameScript.Visitors
{
    public sealed class ErrorVisitor : ViGaSBaseVisitor<object>
    {
        Env env;
        private List<Error> errors;

        private static ErrorVisitor Instance { get; } = new ErrorVisitor();

        private ErrorVisitor()
        {
            env = new Env();
            errors = new List<Error>();
        }

        public static List<Error> CheckErrors(IEnumerable<ScriptFile> files)
        {
            Instance.env = new Env();
            Instance.errors = new List<Error>();

            foreach (var file in files)
            {
                var tree = Executer.ReadAST(file.Document.Text, out var syntaxErrors);
                Instance.errors.AddRange(syntaxErrors);

                Instance.Visit(tree);
            }

            return Instance.errors;
        }

        public override object VisitBaseDefinition([NotNull] BaseDefinitionContext context)
        {
            var baseClass = context.baseClass();
            var baseRef = context.baseRef();

            var baseClassType = TypeSystem.Instance["ErrorType"];
            var instanceClassType = TypeSystem.Instance["ErrorType"];
            if (baseClass != null)
            {
                try
                {
                    baseClassType = TypeSystem.Instance[baseClass.GetText()];
                    instanceClassType = TypeSystem.Instance[$"{baseClass.GetText()}Instance"];
                }
                catch (KeyNotFoundException e)
                {
                    errors.Add(new Error(baseClass, e.Message));
                }
                if (!baseClassType.InheritsFrom(TypeSystem.Instance["GameObject"]))
                    errors.Add(new Error(baseClass, $"Type {baseClassType} is not valid here (must inherit from {TypeSystem.Instance["GameObject"]})."));
            }

            AddSymbolToEnv(baseRef, new Symbol(baseRef.GetText(), baseClassType));

            env = new Env(env, baseRef.GetText());
            AddSymbolToEnv(context, new Symbol("_Base", baseClassType));
            AddSymbolToEnv(context, new Symbol("_Instance", instanceClassType));


            env = new Env(env, $"{baseRef.GetText()} Initialization");
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
            var baseRef = context.baseRef();
            var baseSymbol = GetSymbolFromEnv(baseRef, baseRef.GetText());

            var instanceRef = context.instanceRef();
            var instanceType = TypeSystem.Instance["ErrorType"];

            if (baseSymbol != null)
            {
                if (baseSymbol.Type.InheritsFrom(TypeSystem.Instance["GameObject"]))
                {
                    if (baseSymbol.Type != TypeSystem.Instance["ErrorType"])
                        try
                        {
                            instanceType = TypeSystem.Instance[baseSymbol.Type + "Instance"];
                        }
                        catch (KeyNotFoundException e)
                        {
                            errors.Add(new Error(context, e.Message));
                        }
                }
                else
                    errors.Add(new Error(baseRef, $"Type of {baseRef.GetText()} must be {TypeSystem.Instance["GameObject"]}"));
            }

            if (instanceRef != null)
                AddSymbolToEnv(instanceRef, new Symbol(instanceRef.GetText(), instanceType));

            env = new Env(env, (instanceRef?.GetText() ?? $"Instance of {baseRef.GetText()}") + "Initialization");
            foreach (var prop in instanceType.Properties)
                AddSymbolToEnv(context, new Symbol(prop.Name, prop.Type));

            return base.VisitInstanceDefinition(context);
        }

        public override object VisitRegionDefinition([NotNull] RegionDefinitionContext context)
        {
            var regionRef = context.regionRef();

            env = new Env(env, $"{regionRef.GetText()} Initialization");
            //todo: add region properties to env

            var result = base.VisitRegionDefinition(context);

            return result;
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
                var instanceType = GetSymbolFromEnv(context, "_Instance").Type;
                env = new Env(env, eventCtx.GetText());
                AddSymbolToEnv(context, new Symbol("Self", instanceType));

                foreach (var param in @event.Parameters)
                    AddSymbolToEnv(eventCtx, new Symbol(param.Name, param.Type));

                var result = base.VisitRunBlock(context);

                env = env.Previous;
                return result;
            }
        }

        public override object VisitIfStatement([NotNull] IfStatementContext context)
        {
            var type = TypeVisitor.GetType(context.expression(), env, errors);

            if (!type.InheritsFrom(TypeSystem.Instance["Boolean"]))
                errors.Add(new Error(context, $"Expression type must be {TypeSystem.Instance["Boolean"]}"));

            return base.VisitIfStatement(context);
        }

        public override object VisitRepeatStatement([NotNull] RepeatStatementContext context)
        {
            var type = TypeVisitor.GetType(context.expression(), env, errors);

            if (!type.InheritsFrom(TypeSystem.Instance["Boolean"]))
                errors.Add(new Error(context, $"Expression type must be {TypeSystem.Instance["Boolean"]}"));

            return base.VisitRepeatStatement(context);
        }

        public override object VisitWhileStatement([NotNull] WhileStatementContext context)
        {
            var type = TypeVisitor.GetType(context.expression(), env, errors);

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
                var expressionType = TypeVisitor.GetType(context.expression(), env, errors);

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
            var expressionType = TypeVisitor.GetType(context.expression(), env, errors) ?? TypeSystem.Instance["ErrorType"];
            var symbolType = TypeVisitor.GetType(context.path(), env, errors);

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
                        var paramType = TypeVisitor.GetType(paramsCtx.expression(i), env, errors);
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
