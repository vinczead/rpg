using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using GameScript.Models;
using GameScript.Models.BaseClasses;
using GameScript.Models.InstanceClasses;
using GameScript.Models.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Visitors
{
    public class ExecutionVisitor : ViGaSBaseVisitor<object>
    {
        private Env env;

        private GameModel gameModel;
        private GameObject currentBase;
        private GameObjectInstance currentInstance;
        private Region currentRegion;

        private List<Error> errors;

        /// <summary>
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
        }

        private ExecutionVisitor()
        {
            gameModel = new GameModel();
            env = new Env();
            errors = new List<Error>();
        }

        public static GameModel Build(IEnumerable<IParseTree> trees)
        {
            var worldBuilder = new ExecutionVisitor();
            foreach (var tree in trees)
            {
                worldBuilder.Visit(tree);
            }
            return worldBuilder.gameModel;
        }

        #region World building
        public override object VisitBaseDefinition([NotNull] ViGaSParser.BaseDefinitionContext context)
        {
            var baseRef = context.baseRef().GetText();
            var baseClass = context.baseClass().GetText();

            env = new Env(env, baseRef);
            currentBase = GameObjectFactory.CreateGameObject(baseClass);
            currentBase.Id = baseRef;
            gameModel.Bases.Add(baseRef, currentBase);

            var retVal = base.VisitBaseDefinition(context);

            currentBase = null;

            return retVal;
        }

        public override object VisitBaseBody([NotNull] ViGaSParser.BaseBodyContext context)
        {
            Visit(context.initBlock());

            if (context.variablesBlock() != null)
                Visit(context.variablesBlock());


            foreach (var runBlock in context.runBlock())
            {
                currentBase.RunBlocks[runBlock.eventTypeName().GetText()] = runBlock;
            }

            return null;
        }

        public override object VisitVariableDeclaration([NotNull] ViGaSParser.VariableDeclarationContext context)
        {
            var name = context.varName().GetText();
            var type = context.typeName().GetText();
            string value = "";

            if (context.expression() != null)
                value = Visit(context.expression()).ToString();

            currentBase.Variables[name] = new Symbol(name, TypeSystem.Instance[type], value.ToString());
            return null;
        }

        public override object VisitInstanceDefinition([NotNull] ViGaSParser.InstanceDefinitionContext context)
        {
            var baseRef = context.baseRef().GetText();
            var instanceRef = context.instanceRef()?.GetText();

            currentInstance = gameModel.Spawn(baseRef, currentRegion.Id, instanceRef);

            var retVal = base.VisitInstanceDefinition(context);

            currentInstance = null;

            return retVal;
        }

        public override object VisitRegionDefinition([NotNull] ViGaSParser.RegionDefinitionContext context)
        {
            var regionRef = context.regionRef().GetText();

            currentRegion = new Region() { Id = regionRef, GameModel = gameModel };
            gameModel.Regions.Add(regionRef, currentRegion);

            var retVal = base.VisitRegionDefinition(context);

            currentRegion = null;

            return retVal;
        }

        #endregion

        #region Expressions

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
            return context.GetText();
        }

        public override object VisitRefExpression([NotNull] ViGaSParser.RefExpressionContext context)
        {
            return gameModel.GetById(context.REFERENCE().GetText());    //todo: remove possible $ from start of string
        }

        public override object VisitPathExpression([NotNull] ViGaSParser.PathExpressionContext context)
        {
            return base.VisitPathExpression(context);
        }

        public override object VisitVarPath([NotNull] ViGaSParser.VarPathContext context)
        {
            return base.VisitVarPath(context);
        }

        public override object VisitPath([NotNull] ViGaSParser.PathContext context)
        {
            return base.VisitPath(context);
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
                    return (double)left + (double)right;

                if (@operator.MINUS() != null)
                    return (double)left - (double)right;
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
                return (double)left < (double)right;
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
            var funcNameCtx = context.functionName();
            var paramsCtx = context.functionParameterList();

            var functionName = context.functionName().GetText();
            Console.WriteLine($"Function call {functionName}");
            var parameterList = context.functionParameterList()?.expression().ToList();
            var parameterListValues = parameterList?.Select(p => Visit(p)).ToArray();

            MethodInfo method = typeof(FunctionLibrary).GetMethod(functionName);

            return method.Invoke(null, parameterListValues == null ? null : new object[] { parameterListValues });
        }

        public override object VisitAssignmentStatement([NotNull] ViGaSParser.AssignmentStatementContext context)
        {
            var path = context.path().GetText();
            var value = Visit(context.expression());

            Console.WriteLine($"Assignment: {path} = {value}");

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
    }
}