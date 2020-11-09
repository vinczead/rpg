﻿using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Common.Script.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Common.Script.ViGaSParser;

namespace Common.Script.Visitors
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

        public static List<Error> CheckErrors(string script)
        {
            Instance.env = new Env();
            Instance.errors = new List<Error>();
            var tree = ScriptReader.MakeParseTree(script, out var syntaxErrors);
            Instance.errors.AddRange(syntaxErrors);

            Instance.Visit(tree);

            return Instance.errors;
        }

        public override object VisitTextureDefinition([NotNull] TextureDefinitionContext context)
        {
            //todo: check if file exists
            return base.VisitTextureDefinition(context);
        }

        public override object VisitModelDefinition([NotNull] ModelDefinitionContext context)
        {
            //todo: check if textureId is Texture
            return base.VisitModelDefinition(context);
        }

        public override object VisitAnimationDefinition([NotNull] AnimationDefinitionContext context)
        {
            //todo: check if animationId is unique LOCALLY (may be equal to other ID)
            return base.VisitAnimationDefinition(context);
        }

        public override object VisitFrameDefinition([NotNull] FrameDefinitionContext context)
        {
            //todo: check if all numbers are positive
            return base.VisitFrameDefinition(context);
        }

        public override object VisitTileDefinition([NotNull] TileDefinitionContext context)
        {
            //todo: check if modelId is Model
            return base.VisitTileDefinition(context);
        }

        public override object VisitBaseDefinition([NotNull] BaseDefinitionContext context)
        {
            var baseClass = context.baseClass;
            var baseRef = context.baseRef;

            var baseClassType = TypeSystem.Instance["ErrorType"];
            var instanceClassType = TypeSystem.Instance["ErrorType"];
            if (baseClass != null)
            {
                try
                {
                    baseClassType = TypeSystem.Instance[baseClass.Text];
                    instanceClassType = TypeSystem.Instance[$"{baseClass.Text}Instance"];
                }
                catch (KeyNotFoundException e)
                {
                    errors.Add(new Error(baseClass, e.Message));
                }
                if (!baseClassType.InheritsFrom(TypeSystem.Instance["GameObject"]))
                    errors.Add(new Error(baseClass, $"Type {baseClassType} is not valid here (must inherit from {TypeSystem.Instance["GameObject"]})."));
            }

            AddSymbolToEnv(baseRef, new Symbol(baseRef.Text, baseClassType));

            env = new Env(env, baseRef.Text);
            AddSymbolToEnv(context, new Symbol("_Base", baseClassType));
            AddSymbolToEnv(context, new Symbol("_Instance", instanceClassType));


            env = new Env(env, $"{baseRef.Text} Initialization");
            AddSymbolToEnv(context, new Symbol("Self", baseClassType));

            //            foreach (var prop in baseClassType.Properties)
            //                AddSymbolToEnv(baseClass, new Symbol(prop.Name, prop.Type));

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
            var baseRef = context.baseRef;
            var baseSymbol = GetSymbolFromEnv(baseRef, baseRef.Text);

            var instanceRef = context.instanceRef;
            var instanceType = TypeSystem.Instance["ErrorType"];

            //todo: check if x,y in bounds

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
                    errors.Add(new Error(baseRef, $"Type of {baseRef.Text} must be {TypeSystem.Instance["GameObject"]}"));
            }

            if (instanceRef != null)
                AddSymbolToEnv(instanceRef, new Symbol(instanceRef.Text, instanceType));

            env = new Env(env, (instanceRef?.Text ?? $"Instance of {baseRef.Text}") + "Initialization");
            AddSymbolToEnv(context, new Symbol("Self", instanceType, instanceRef?.Text ?? ""));
            //            foreach (var prop in instanceType.Properties)
            //                AddSymbolToEnv(context, new Symbol(prop.Name, prop.Type));

            return base.VisitInstanceDefinition(context);
        }

        public override object VisitPlayerDefinition([NotNull] PlayerDefinitionContext context)
        {
            var instanceId = context.instanceId.Text;
            var instanceSymbol = GetSymbolFromEnv(context, instanceId);
            if (instanceSymbol != null && instanceSymbol.Type != TypeSystem.Instance["CharacterInstance"])
            {
                errors.Add(new Error(context.instanceId, $"Type mismatch: the player keyword can only be used with Instances of Characters."));
            }
            return base.VisitPlayerDefinition(context);
        }

        public override object VisitRegionDefinition([NotNull] RegionDefinitionContext context)
        {
            var regionRef = context.regionRef.Text;
            var width = context.width.Text;
            var height = context.height.Text;

            if (int.TryParse(width, out int widthValue))
            {
                if (widthValue <= 0)
                    errors.Add(new Error(context.width, $"Invalid value: width must greater than 0."));
            }
            else
            {
                errors.Add(new Error(context.width, $"Type mismatch: width must be an integer."));
            }

            if (widthValue <= 0)
            {
                errors.Add(new Error(context.width, $"Invalid value: width must greater than 0."));
            }

            if (int.TryParse(height, out int heightValue))
            {
                if (heightValue <= 0)
                    errors.Add(new Error(context.width, $"Invalid value: height must greater than 0."));
            }
            else
            {
                errors.Add(new Error(context.width, $"Type mismatch: height must be an integer."));
            }

            AddSymbolToEnv(context, new Symbol(regionRef, TypeSystem.Instance["Region"]));

            return base.VisitRegionDefinition(context);
        }

        public override object VisitTilesBlock([NotNull] TilesBlockContext context)
        {
            var expressions = context.expression()?.ToList() ?? new List<ExpressionContext>();

            foreach (var expression in expressions)
            {
                var type = TypeVisitor.GetType(expression, env, errors);
                if (type != TypeSystem.Instance["TileArray"])
                {
                    errors.Add(new Error(expression, "Type mismatch: must be array of Tile."));
                } else
                {
                    //todo check width
                }
            }
            //todo check height
            return base.VisitTilesBlock(context);
        }

        public override object VisitVariableDeclaration([NotNull] VariableDeclarationContext context)
        {
            //check whether expression after WithValue matches type
            var varName = context.VARNAME()?.GetText();

            var varType = TypeSystem.Instance["ErrorType"];

            try
            {
                varType = TypeSystem.Instance[context.typeName?.Text];
            }
            catch (KeyNotFoundException e)
            {
                errors.Add(new Error(context.typeName, e.Message));
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

        public override object VisitRunBlock([NotNull] RunBlockContext context)
        {
            var eventToken = context.eventTypeName;

            var baseType = GetSymbolFromEnv(context, "_Base").Type;

            var @event = baseType.Events.FirstOrDefault(e => e.Name == eventToken.Text);

            if (@event == null)
            {
                errors.Add(new Error(eventToken, $"Event {eventToken.Text} is not defined on {baseType}."));
                return base.VisitRunBlock(context);
            }
            else
            {
                var instanceType = GetSymbolFromEnv(context, "_Instance").Type;
                env = new Env(env, eventToken.Text);
                AddSymbolToEnv(context, new Symbol("Self", instanceType));

                foreach (var param in @event.Parameters)
                    AddSymbolToEnv(eventToken, new Symbol(param.Name, param.Type));

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

        public override object VisitFunctionCallStatement([NotNull] FunctionCallStatementContext context)
        {
            //check whether function exists and parameter types match
            var funcNameToken = context.functionName;

            try
            {
                var function = FunctionManager.Instance[funcNameToken.Text];

                var paramsCtx = context.functionParameterList();
                var requiredParamCount = function.Parameters.Count;
                var actualParamCount = paramsCtx?.expression()?.Length ?? 0;

                if (actualParamCount == requiredParamCount)
                {
                    for (int i = 0; i < function.Parameters.Count; i++)
                    {
                        var paramType = TypeVisitor.GetType(paramsCtx.expression(i), env, errors);
                        if (!paramType.InheritsFrom(function.Parameters[i].Type))
                            errors.Add(new Error(funcNameToken, $"Type mismatch: The function {function} expects {function.Parameters[i].Type} as parameter {i + 1}, not {paramType}."));
                    }
                }
                else
                    errors.Add(new Error(funcNameToken, $"The function {function} requires {requiredParamCount} parameter(s), not {actualParamCount}."));
            }
            catch (KeyNotFoundException e)
            {
                errors.Add(new Error(funcNameToken, e.Message));
            }

            return base.VisitFunctionCallStatement(context);
        }

        public override object VisitAssignmentStatement([NotNull] AssignmentStatementContext context)
        {
            var pathType = TypeVisitor.GetType(context.path(), env, errors);
            var expressionType = TypeVisitor.GetType(context.expression(), env, errors) ?? TypeSystem.Instance["ErrorType"];

            if (pathType != TypeSystem.Instance["ErrorType"] && !expressionType.InheritsFrom(pathType))
            {
                errors.Add(new Error(context.expression(), $"Type mismatch: {expressionType} cannot be converted to {pathType}."));
            }

            return base.VisitAssignmentStatement(context);
        }

        private void AddSymbolToEnv(IToken token, Symbol symbol)
        {
            try
            {
                env[symbol.Name] = symbol;
            }
            catch
            {
                errors.Add(new Error(token, $"{symbol.Name} is already defined."));
            }
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

        private Symbol GetSymbolFromEnv(IToken token, string symbolName)
        {
            if (symbolName == null)
                return null;

            var symbol = env[symbolName];
            if (symbol != null)
                return symbol;

            errors.Add(new Error(token, $"{symbolName} does not exist in this context."));
            return null;
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