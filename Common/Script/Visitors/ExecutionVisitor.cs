using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Common.Models;
using Common.Script.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Script.Visitors
{
    public sealed class ExecutionVisitor : ViGaSBaseVisitor<object>
    {
        private Env env;

        private Thing currentBreed;
        private ThingInstance currentInstance;
        private SpriteModel currentModel;
        private Animation currentAnimation;
        private Region currentRegion;

        private List<Error> errors;

        private static ExecutionVisitor Instance { get; } = new ExecutionVisitor();

        /*/// <summary>
        /// This constructor should be used in game code when executing Run blocks
        /// </summary>
        /// <param name="gameModel"></param>
        /// <param name="currentInstance"></param>
        /// <param name="errors"></param>
        public ExecutionVisitor(GameModel gameModel, GameObjectInstance currentInstance, List<Error> errors = null)
        {
            this.env = new Env();   //todo: create Env from gameModel and currentInstance
            this.errors = errors ?? new List<Error>();
            this.gameModel = gameModel;
            this.currentInstance = currentInstance;
        }*/

        private ExecutionVisitor()
        {
            World.Instance.Clear();
            env = new Env();
            errors = new List<Error>();
        }

        public static void BuildWorld(IEnumerable<string> files, out List<Error> errors)
        {
            Instance.errors = new List<Error>();
            var worldBuilder = new ExecutionVisitor();
            foreach (var file in files)
            {
                var tree = ScriptReader.ReadAST(file, out _);
                worldBuilder.Visit(tree);
            }
            errors = Instance.errors;
        }

        public static List<Error> ExecuteRunBlock(ThingInstance currentInstance, string runBlockId, List<Symbol> parameters = null)
        {
            Instance.env = new Env(World.Instance.ToEnv(), runBlockId);

            if (parameters != null)
                foreach (var p in parameters)
                    Instance.env[p.Name] = p;

            //todo: actually run the run block
            /*if (currentInstance.Breed.RunBlocks.TryGetValue(runBlockId, out var runBlock))
                Instance.Visit(runBlock);*/
            return Instance.errors;
        }

        #region World building

        public override object VisitTextureDefinition([NotNull] ViGaSParser.TextureDefinitionContext context)
        {
            var id = context.textureId.Text;
            var fileName = context.fileName.Text[1..^1];

            if (World.Instance.Game != null)
                World.Instance.LoadTextureFromFile(id, fileName);
            else {
                World.Instance.Textures.Add(id, null);
                errors.Add(new Error(context, $"Warning: Texture {id} from file {fileName} was not loaded, because Game is not initialized."));
            }

            return null;
        }

        public override object VisitModelDefinition([NotNull] ViGaSParser.ModelDefinitionContext context)
        {
            var modelId = context.modelId.Text;
            var textureId = context.textureId.Text;
            AddSymbolToEnv(context, new Symbol(modelId, TypeSystem.Instance["Model"], modelId));
            currentModel = new SpriteModel()
            {
                Id = modelId,
                SpriteSheet = World.Instance.Textures[textureId]
            };
            World.Instance.Models.Add(modelId, currentModel);

            var retVal = base.VisitModelDefinition(context);
            currentModel = null;
            return retVal;
        }

        public override object VisitAnimationDefinition([NotNull] ViGaSParser.AnimationDefinitionContext context)
        {
            var animationId = context.animationId.Text;
            var looping = context.LOOPING() != null;

            currentAnimation = new Animation()
            {
                Id = animationId,
                IsLooping = looping
            };
            currentModel.Animations.Add(currentAnimation);

            var retVal = base.VisitAnimationDefinition(context);

            currentAnimation = null;

            return retVal;
        }

        public override object VisitFrameDefinition([NotNull] ViGaSParser.FrameDefinitionContext context)
        {
            var x = int.Parse(context.x.Text);
            var y = int.Parse(context.y.Text);
            var width = int.Parse(context.width.Text);
            var height = int.Parse(context.height.Text);
            var duration = int.Parse(context.duration.Text);
            var frame = new Frame()
            {
                Source = new Rectangle(x, y, width, height),
                TimeSpan = TimeSpan.FromMilliseconds(duration)
            };
            currentAnimation.Frames.Add(frame);

            return null;
        }

        public override object VisitTileDefinition([NotNull] ViGaSParser.TileDefinitionContext context)
        {
            var id = context.tileId.Text;
            var modelId = context.modelId.Text;
            var tile = new Tile()
            {
                Id = id,
                Model = World.Instance.Models[modelId]
            };

            World.Instance.Tiles.Add(id, tile);
            return null;
        }

        public override object VisitBaseDefinition([NotNull] ViGaSParser.BaseDefinitionContext context)
        {
            var baseRef = context.baseRef.Text;
            var baseClass = context.baseClass.Text;

            AddSymbolToEnv(context, new Symbol(baseRef, TypeSystem.Instance[baseClass], baseRef));
            env = new Env(env, baseRef);
            currentBreed = GameObjectFactory.Create(baseClass);
            currentBreed.Id = baseRef;
            World.Instance.Breeds.Add(baseRef, currentBreed);

            AddSymbolToEnv(context, new Symbol("Self", TypeSystem.Instance[baseClass], baseRef));

            var retVal = base.VisitBaseDefinition(context);

            currentBreed = null;

            return retVal;
        }

        public override object VisitBaseBody([NotNull] ViGaSParser.BaseBodyContext context)
        {
            Visit(context.initBlock());

            if (context.variablesBlock() != null)
                Visit(context.variablesBlock());


            foreach (var runBlock in context.runBlock())
            {
                //currentBreed.RunBlocks[runBlock.eventTypeName.Text] = runBlock; //todo: actually save run block
            }

            return null;
        }

        public override object VisitVariableDeclaration([NotNull] ViGaSParser.VariableDeclarationContext context)
        {
            var name = context.varName.Text;
            var type = context.typeName.Text;
            string value = "";

            if (context.expression() != null)
                value = Visit(context.expression()).ToString();

            //currentBreed.Variables[name] = new Symbol(name, TypeSystem.Instance[type], value.ToString()); //todo: actually save variable
            return null;
        }

        public override object VisitInstanceDefinition([NotNull] ViGaSParser.InstanceDefinitionContext context)
        {
            var baseRef = context.baseRef.Text;
            var instanceRef = context.instanceRef?.Text;
            var isPlayer = context.PLAYER() != null;
            var x = float.Parse(context.x.Text);
            var y = float.Parse(context.y.Text);

            var baseSymbol = GetSymbolFromEnv(context, baseRef);
            var instanceType = TypeSystem.Instance[baseSymbol.Type + "Instance"]; //todo: this could be nicer - maybe read instanceType from json?

            AddSymbolToEnv(context, new Symbol(instanceRef, instanceType, instanceRef));
            env = new Env(env, instanceRef ?? $"Instance of {baseRef}");
            currentInstance = World.Instance.Spawn(baseRef, currentRegion.Id, instanceRef);
            currentInstance.Region = currentRegion;
            currentRegion.instances.Add(currentInstance);
            currentInstance.Position = new Vector2(x, y);
            if (isPlayer)
                World.Instance.Player = currentInstance as CharacterInstance;

            AddSymbolToEnv(context, new Symbol("Self", instanceType, currentInstance.Id));

            var retVal = base.VisitInstanceDefinition(context);

            currentInstance = null;

            return retVal;
        }

        public override object VisitRegionDefinition([NotNull] ViGaSParser.RegionDefinitionContext context)
        {
            var regionRef = context.regionRef.Text;

            AddSymbolToEnv(context, new Symbol(regionRef, TypeSystem.Instance["Region"], regionRef));
            env = new Env(env, regionRef);
            currentRegion = new Region() { Id = regionRef };
            World.Instance.Regions.Add(regionRef, currentRegion);

            AddSymbolToEnv(context, new Symbol("Self", TypeSystem.Instance["Region"], currentRegion.Id));

            var retVal = base.VisitRegionDefinition(context);

            currentRegion = null;

            return retVal;
        }

        public override object VisitInitBlock([NotNull] ViGaSParser.InitBlockContext context)
        {
            var retVal = base.VisitInitBlock(context);

            env = env.Previous;

            return retVal;
        }

        #endregion

        #region Expressions
        public override object VisitArrayExpression([NotNull] ViGaSParser.ArrayExpressionContext context)
        {
            return context.expression().Select(expression => Visit(expression)).ToArray();
        }

        public override object VisitBoolExpression([NotNull] ViGaSParser.BoolExpressionContext context)
        {
            return bool.Parse(context.GetText());
        }

        public override object VisitNumberExpression([NotNull] ViGaSParser.NumberExpressionContext context)
        {
            return double.Parse(context.GetText());
        }

        public override object VisitNullExpression([NotNull] ViGaSParser.NullExpressionContext context)
        {
            return null;
        }

        public override object VisitStringExpression([NotNull] ViGaSParser.StringExpressionContext context)
        {
            var str = context.GetText();
            return str.Substring(1, str.Length - 2);
        }

        public override object VisitRefExpression([NotNull] ViGaSParser.RefExpressionContext context)
        {
            return World.Instance.GetById(context.REFERENCE().GetText().Substring(1));
        }

        public override object VisitParamExpression([NotNull] ViGaSParser.ParamExpressionContext context)
        {
            var symbol = GetSymbolFromEnv(context, context.param.Text);
            return World.Instance.GetById(symbol?.Value);
        }

        public override object VisitPathExpression([NotNull] ViGaSParser.PathExpressionContext context)
        {
            var pathValue = Visit(context.path());

            if (pathValue is PropInfoPathValue)
            {
                var propInfoPathValue = pathValue as PropInfoPathValue;

                var gameObject = World.Instance.GetById(propInfoPathValue.Symbol.Value);
                var propInfo = propInfoPathValue.PropertyInfo;

                return propInfo.GetValue(gameObject);
            }

            if (pathValue is Symbol)
            {
                return pathValue;
            }

            return null;
        }

        public override object VisitPath([NotNull] ViGaSParser.PathContext context)
        {
            Symbol currentSymbol = null;

            PropertyInfo thisPropertyInfo = null;

            if (context.param != null)
                currentSymbol = GetSymbolFromEnv(context.param, context.param.Text);
            if (context.@ref != null)
                currentSymbol = GetSymbolFromEnv(context.@ref, context.@ref.Text);

            if (currentSymbol == null)
                return null;

            for (int i = 0; i < context._parts.Count; i++)
            {
                IToken part = context._parts[i];
                bool isLastPart = i == context._parts.Count - 1;

                //this part is a variable
                if (char.IsLower(part.Text[0]))
                {
                    //only variables of Instance classes can be accessed
                    if (currentSymbol.Type.InheritsFrom(TypeSystem.Instance["ThingInstance"]))
                    {
                        var gameObject = World.Instance.GetById(currentSymbol.Value) as ThingInstance;

                        //currentSymbol = gameObject.Variables[part.Text]; //todo: actually reach variable

                        if (isLastPart)
                            return currentSymbol;
                    }
                    else
                    {
                        errors.Add(new Error(part, $"Only variables of Instances can be accessed."));
                        return null;
                    }
                }
                else //this part is a property
                {
                    var gameObject = World.Instance.GetById(currentSymbol.Value); //Get the Breed that corresponds to the current symbol

                    thisPropertyInfo = gameObject.GetType().GetProperty(part.Text); //The PropertyInfo that is being referenced by 'part'

                    if (isLastPart)
                        return new PropInfoPathValue()
                        {
                            PropertyInfo = thisPropertyInfo,
                            Symbol = currentSymbol
                        };

                    //if this isn't the last part, and the property referenced by part is a Breed or Instance, resolve the corresponding symbol
                    var thisPropertyType = thisPropertyInfo.PropertyType;
                    if (thisPropertyType.IsAssignableFrom(typeof(Thing)) || thisPropertyType.IsAssignableFrom(typeof(ThingInstance)))
                    {
                        //get the Id of this Breed or Instance
                        var gameObjectIdPropertyInfo = thisPropertyInfo.PropertyType.GetProperty("Id");
                        var currentPropertyValue = thisPropertyInfo.GetValue(gameObject);

                        //and get the symbol for this id
                        currentSymbol = GetSymbolFromEnv(part, gameObjectIdPropertyInfo.GetValue(currentPropertyValue).ToString());
                    }
                    else
                    {
                        errors.Add(new Error(part, $"Only properties of Instances and Breeds can be accessed."));
                        return null;
                    }
                }
            }

            return null;
        }

        public override object VisitParenExpression([NotNull] ViGaSParser.ParenExpressionContext context)
        {
            return Visit(context.expression());
        }

        public override object VisitAdditiveExpression([NotNull] ViGaSParser.AdditiveExpressionContext context)
        {
            var left = Visit(context.left);
            var right = Visit(context.right);

            var leftType = TypeVisitor.GetType(context.left, env, errors);
            var rightType = TypeVisitor.GetType(context.right, env, errors);

            var @operator = context.additiveOperator();

            if (leftType.InheritsFrom(TypeSystem.Instance["String"]) && @operator.PLUS() != null)
                return string.Concat(left, right);

            if (leftType == rightType && leftType.InheritsFrom(TypeSystem.Instance["Number"]))
            {
                if (@operator.PLUS() != null)
                    return (double)Convert.ChangeType(left, typeof(double)) + (double)Convert.ChangeType(right, typeof(double));

                if (@operator.MINUS() != null)
                    return (double)Convert.ChangeType(left, typeof(double)) - (double)Convert.ChangeType(right, typeof(double));
            }

            throw new InvalidOperationException($"Cannot evaluate additive operation.");
        }

        public override object VisitMultiplExpression([NotNull] ViGaSParser.MultiplExpressionContext context)
        {
            var left = Visit(context.left);
            var right = Visit(context.right);

            var leftType = TypeVisitor.GetType(context.left, env, errors);
            var rightType = TypeVisitor.GetType(context.right, env, errors);

            var @operator = context.multiplOperator();

            if (leftType == rightType && leftType.InheritsFrom(TypeSystem.Instance["Number"]))
            {
                if (@operator.MULT() != null)
                    return (double)left * (double)right;

                if (@operator.DIV() != null)
                    return (double)left / (double)right;
            }

            throw new InvalidOperationException($"Cannot evaluate multiplicative operation.");
        }

        public override object VisitCompExpression([NotNull] ViGaSParser.CompExpressionContext context)
        {
            var left = Visit(context.left);
            var right = Visit(context.right);

            var leftType = TypeVisitor.GetType(context.left, env, errors);
            var rightType = TypeVisitor.GetType(context.right, env, errors);

            var op = context.compOperator();

            if (leftType != rightType)
                throw new InvalidOperationException($"Cannot execute operation '{op.GetText()}' on ${left} and ${right}");

            if (op.EQ() != null)
                return left == right;

            if (op.NEQ() != null)
                return left != right;

            if (leftType != TypeSystem.Instance["Number"])
                throw new InvalidOperationException($"Cannot execute operation '{op.GetText()}' on ${left} and ${right}");

            if (op.LT() != null)
                return (double)Convert.ChangeType(left, typeof(double)) < (double)Convert.ChangeType(right, typeof(double));
            if (op.GT() != null)
                return (double)left > (double)right;
            if (op.LTE() != null)
                return (double)left <= (double)right;
            if (op.GTE() != null)
                return (double)left >= (double)right;

            throw new InvalidOperationException($"Cannot evaluate comparative operation.");
        }

        public override object VisitLogicalExpression([NotNull] ViGaSParser.LogicalExpressionContext context)
        {
            var left = Visit(context.left);
            var right = Visit(context.right);

            var leftType = TypeVisitor.GetType(context.left, env, errors);
            var rightType = TypeVisitor.GetType(context.right, env, errors);

            var op = context.logicalOperator();

            if (leftType == rightType && leftType.InheritsFrom(TypeSystem.Instance["Boolean"]))
            {
                if (op.AND() != null)
                    return (bool)left & (bool)right;
                if (op.OR() != null)
                    return (bool)left | (bool)right;
                if (op.XOR() != null)
                    return (bool)left ^ (bool)right;
            }

            throw new InvalidOperationException($"Cannot evaluate logical operation.");
        }

        public override object VisitNotExpression([NotNull] ViGaSParser.NotExpressionContext context)
        {
            var expression = Visit(context.expression());
            var type = TypeVisitor.GetType(context.expression(), env, errors);

            if (type.InheritsFrom(TypeSystem.Instance["Boolean"]))
                return !(bool)expression;

            throw new InvalidOperationException($"Cannot evaluate not operation.");
        }

        public override object VisitFuncExpression([NotNull] ViGaSParser.FuncExpressionContext context)
        {
            return base.VisitFuncExpression(context);
        }

        #endregion

        #region Statements

        public override object VisitFunctionCallStatement([NotNull] ViGaSParser.FunctionCallStatementContext context)
        {
            //check whether function exists and parameter types match
            var paramsCtx = context.functionParameterList();
            var ctxstr = context.GetText();

            var functionName = context.functionName.Text;
            var parameterList = context.functionParameterList()?.expression().ToList();
            var parameterListValues = parameterList?.Select(p => Visit(p)).ToArray();

            MethodInfo method = typeof(FunctionLibrary).GetMethod(functionName);

            var retval = method.Invoke(null, parameterListValues == null ? null : new object[] { parameterListValues });

            return retval;
        }

        public override object VisitAssignmentStatement([NotNull] ViGaSParser.AssignmentStatementContext context)
        {
            var pathValue = Visit(context.path());
            var expressionValue = Visit(context.expression());

            if (pathValue is PropInfoPathValue)
            {
                var propInfoPathValue = pathValue as PropInfoPathValue;

                var gameObject = World.Instance.GetById(propInfoPathValue.Symbol.Value);
                var propInfo = propInfoPathValue.PropertyInfo;

                propInfo.SetValue(gameObject, Convert.ChangeType(expressionValue, propInfo.PropertyType));
            }

            if (pathValue is Symbol)
            {
                var symbol = pathValue as Symbol;

                symbol.Value = expressionValue.ToString();
            }

            return null;
        }

        public override object VisitIfStatement([NotNull] ViGaSParser.IfStatementContext context)
        {
            var expression = (bool)Visit(context.expression());

            if (expression)
                Visit(context.statementList());
            else
                if (context.elseStatement() != null)
                Visit(context.elseStatement().statementList());

            return null;
        }

        public override object VisitRepeatStatement([NotNull] ViGaSParser.RepeatStatementContext context)
        {
            do
            {
                Visit(context.statementList());
            } while ((bool)Visit(context.expression()));

            return null;
        }

        public override object VisitWhileStatement([NotNull] ViGaSParser.WhileStatementContext context)
        {
            while ((bool)Visit(context.expression()))
            {
                Visit(context.statementList());
            }

            return null;
        }
        #endregion

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
    }
}
