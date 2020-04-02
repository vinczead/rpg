//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using Antlr4.Runtime.Misc;
//using GameModel.Models;
//using GameModel.Models.InstanceInterfaces;
//using static GameScript.ViGaSParser;

//namespace GameScript.Visitors
//{
//    public static class TypeChecker
//    {
//        public static Type GetTypeOfFunctionCallStatement([NotNull] FunctionCallStatementContext context, IGameWorldObject gameObject)
//        {
//            /*var path = context.path();
//            var functionName = context.functionName().GetText();
//            var parameterList = context.functionParameterList()?.expression().ToList();
//            var parameterListTypes = parameterList?.Select(p => TypeChecker.GetTypeOf(p, gameObject)).ToArray();

//            object subject = gameObject.World;
//            if (path != null)
//            {
//                var expressionVisitor = new ExpressionVisitor(gameObject);
//                subject = expressionVisitor.VisitPath(path);
//            }

//            MethodInfo method = subject.GetType().GetMethod(functionName, parameterListTypes ?? new Type[0]);
//            return method.ReturnType;*/
//            return typeof(bool);
//        }

//        public static Type GetTypeOfTypeName([NotNull] TypeNameContext context)
//        {
//            switch (context.GetText())
//            {
//                case "Boolean":
//                    return typeof(bool);
//                case "Number":
//                    return typeof(double);
//                case "String":
//                    return typeof(string);
//                case "Reference":
//                    return typeof(ReferenceType);
//                default:
//                    return typeof(ErrorType);
//            }
//        }

//        public static Type GetTypeOfRefExpression([NotNull] RefExpressionContext context)
//        {
//            return typeof(ReferenceType);
//        }

//        public static Type GetTypeOfStringExpression([NotNull] StringExpressionContext context)
//        {
//            return typeof(string);
//        }

//        public static Type GetTypeOfBoolExpression([NotNull] BoolExpressionContext context)
//        {
//            return typeof(bool);
//        }

//        public static Type GetTypeOfNumberExpression([NotNull] NumberExpressionContext context)
//        {
//            return typeof(double);
//        }

//        public static Type GetTypeOfNotExpression([NotNull] NotExpressionContext context, IGameWorldObject gameObject)
//        {
//            if (GetTypeOf(context.expression(), gameObject) == typeof(bool))
//                return typeof(bool);
//            else
//                return typeof(ErrorType);
//        }

//        public static Type GetTypeOfParenExpression([NotNull] ParenExpressionContext context, IGameWorldObject gameObject)
//        {
//            return GetTypeOf(context.expression(), gameObject);
//        }

//        public static Type GetTypeOfCompExpression([NotNull] CompExpressionContext context, IGameWorldObject gameObject)
//        {
//            var leftType = GetTypeOf(context.left, gameObject);
//            var rightType = GetTypeOf(context.right, gameObject);
//            var @operator = context.compOperator();

//            if (leftType != rightType)
//                return typeof(ErrorType);

//            //operators other than == and != are only valid on numbers
//            if (@operator.EQ() == null && @operator.NEQ() == null && leftType != typeof(double))
//                return typeof(ErrorType);

//            return typeof(bool);
//        }

//        public static Type GetTypeOfAdditiveExpression([NotNull] AdditiveExpressionContext context, IGameWorldObject gameObject)
//        {
//            var leftType = GetTypeOf(context.left, gameObject);
//            var rightType = GetTypeOf(context.right, gameObject);
//            var @operator = context.additiveOperator();

//            if (leftType == typeof(string) && @operator.PLUS() != null)
//                return leftType;

//            if (leftType == rightType && leftType == typeof(double))
//                return leftType;

//            return typeof(ErrorType);
//        }

//        public static Type GetTypeOfMultiplExpression([NotNull] MultiplExpressionContext context, IGameWorldObject gameObject)
//        {
//            var leftType = GetTypeOf(context.left, gameObject);
//            var rightType = GetTypeOf(context.right, gameObject);

//            if (leftType == rightType && leftType == typeof(double))
//                return leftType;

//            return typeof(ErrorType);
//        }

//        public static Type GetTypeOfLogicalExpression([NotNull] LogicalExpressionContext context, IGameWorldObject gameObject)
//        {
//            var leftType = GetTypeOf(context.left, gameObject);
//            var rightType = GetTypeOf(context.right, gameObject);

//            if (leftType == rightType && leftType == typeof(bool))
//                return leftType;

//            return typeof(ErrorType);
//        }

//        public static Type GetTypeOfPathExpression([NotNull] PathExpressionContext context, IGameWorldObject gameObject)
//        {
//            return GetTypeOfPath(context.path(), gameObject);
//        }

//        private static Type GetTypeOfPath(PathContext context, IGameWorldObject gameObject)
//        {
//            var varPath = context.varPath();
//            var refPath = context.refPath();

//            if (varPath != null)
//            {
//                return VisitVarPath(varPath, gameObject).Type;
//            }

//            if (refPath != null)
//            {
//                return VisitRefPath(refPath, gameObject).Type;
//            }

//            return null;
//        }

//        private static Variable VisitRefPath(RefPathContext refPath, IGameWorldObject gameObject)
//        {
//            throw new NotImplementedException();
//        }

//        private static Variable VisitVarPath(VarPathContext context, IGameWorldObject gameObject)
//        {
//            Variable variable = null;
//            IGameWorldObject currentObject = gameObject;

//            foreach (var varName in context.varName())
//            {
//                if (currentObject == null)
//                    throw new IdNotFoundException(varName.GetText());

//                if (currentObject.Variables.TryGetValue(varName.GetText(), out variable))
//                {
//                    if (variable.Type == typeof(ReferenceType))
//                        currentObject = (IGameWorldObject)gameObject.World.GetById(variable.Name);
//                    else
//                        currentObject = null;
//                }
//                else
//                    throw new IdNotFoundException(varName.GetText());
//            }

//            return variable;
//        }

//        public static Type GetTypeOf([NotNull]ExpressionContext context, IGameWorldObject gameObject)
//        {
//            if (context is ParenExpressionContext)
//                return GetTypeOfParenExpression(context as ParenExpressionContext, gameObject);

//            if (context is PathExpressionContext)
//                return GetTypeOfPathExpression(context as PathExpressionContext, gameObject);

//            if (context is RefExpressionContext)
//                return GetTypeOfRefExpression(context as RefExpressionContext);

//            if (context is StringExpressionContext)
//                return GetTypeOfStringExpression(context as StringExpressionContext);

//            if (context is BoolExpressionContext)
//                return GetTypeOfBoolExpression(context as BoolExpressionContext);

//            if (context is NumberExpressionContext)
//                return GetTypeOfNumberExpression(context as NumberExpressionContext);

//            if (context is NotExpressionContext)
//                return GetTypeOfNotExpression(context as NotExpressionContext, gameObject);

//            if (context is CompExpressionContext)
//                return GetTypeOfCompExpression(context as CompExpressionContext, gameObject);

//            if (context is AdditiveExpressionContext)
//                return GetTypeOfAdditiveExpression(context as AdditiveExpressionContext, gameObject);

//            if (context is MultiplExpressionContext)
//                return GetTypeOfMultiplExpression(context as MultiplExpressionContext, gameObject);

//            if (context is LogicalExpressionContext)
//                return GetTypeOfLogicalExpression(context as LogicalExpressionContext, gameObject);

//            if (context is FuncExpressionContext)
//                return GetTypeOfFunctionCallStatement((context as FuncExpressionContext).functionCallStatement(), gameObject);

//            throw new ArgumentException("Invalid parameter", "context");

//        }

//    }
//}
