using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using GameScript.Models;
using GameScript.Models.BaseClasses;
using GameScript.Models.InstanceClasses;
using GameScript.Models.Script;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Visitors
{
    /// <summary>
    /// Builds a script from the scripts given
    /// </summary>
    public class WorldBuilderVisitor : ViGaSBaseVisitor<object>
    {
        private Env env;

        private GameModel gameModel;
        private GameObject currentBase;
        private GameObjectInstance currentInstance;
        private Region currentRegion;

        private List<Error> errors;

        private WorldBuilderVisitor()
        {
            gameModel = new GameModel();
            env = new Env();
            errors = new List<Error>();
        }

        public static GameModel Build(IEnumerable<IParseTree> trees)
        {
            var worldBuilder = new WorldBuilderVisitor();
            foreach (var tree in trees)
            {
                worldBuilder.Visit(tree);
            }
            return worldBuilder.gameModel;
        }

        public override object VisitBaseDefinition([NotNull] ViGaSParser.BaseDefinitionContext context)
        {
            var baseId = context.baseId().GetText();
            var baseClass = context.baseClass().GetText();

            env = new Env(env, baseId);
            currentBase = GameObjectFactory.CreateGameObject(baseClass);
            currentBase.Id = baseId;
            gameModel.Bases.Add(baseId, currentBase);

            var retVal = base.VisitBaseDefinition(context);

            currentBase = null;

            return retVal;
        }

        public override object VisitBaseBody([NotNull] ViGaSParser.BaseBodyContext context)
        {
            return base.VisitBaseBody(context);
        }

        public override object VisitVariableDeclaration([NotNull] ViGaSParser.VariableDeclarationContext context)
        {
            var name = context.varName().GetText();
            var type = context.typeName().GetText();
            var value = Visit(context.expression());

            currentBase.Variables[name] = new Symbol(name, TypeSystem.Instance[type], value.ToString());
            return null;
        }

        public override object VisitRunBlock([NotNull] ViGaSParser.RunBlockContext context)
        {
            //Store run block subtree in base object
            currentBase.RunBlocks[context.eventTypeName().GetText()] = context;
            return null;
        }

        public override object VisitInstanceDefinition([NotNull] ViGaSParser.InstanceDefinitionContext context)
        {
            var baseId = context.baseId().GetText();
            var instanceId = context.instanceId().GetText();

            currentInstance = gameModel.Bases[baseId].Spawn(instanceId);

            gameModel.Instances.Add(instanceId, currentInstance);

            var retVal = base.VisitInstanceDefinition(context);

            currentInstance = null;

            return retVal;
        }
    }
}