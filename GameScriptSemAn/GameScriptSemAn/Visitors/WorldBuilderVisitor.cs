using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using GameScript.Models;
using GameScript.Models.BaseClasses;
using GameScript.Models.InstanceClasses;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Visitors
{
    public class WorldBuilderVisitor : ViGaSBaseVisitor<object>
    {
        public World World { get; private set; }

        private string script;
        private GameObject currentBase;
        private GameObjectInstance currentInstance;

        public WorldBuilderVisitor()
        {
            World = new World();
        }

        public override object VisitBaseDefinition([NotNull] ViGaSParser.BaseDefinitionContext context)
        {
            var baseId = context.baseHeader().baseId().GetText();
            var baseClass = context.baseHeader().baseClass().GetText();

            currentBase = GameObjectFactory.CreateGameObject(baseClass);
            currentBase.Id = baseId;
            currentBase.Script = script;
            World.Bases.Add(baseId, currentBase);

            var retVal = base.VisitBaseDefinition(context);

            currentBase = null;

            return retVal;
        }

        public override object VisitVariableDeclaration([NotNull] ViGaSParser.VariableDeclarationContext context)
        {
            if (currentInstance == null)
                return null;

            return base.VisitVariableDeclaration(context);
        }

        public override object VisitInitBlock([NotNull] ViGaSParser.InitBlockContext context)
        {

            return base.VisitInitBlock(context);
        }

        public override object VisitRunBlock([NotNull] ViGaSParser.RunBlockContext context)
        {
            //Run blocks are skipped
            return null;
        }

        public override object VisitInstanceDefinition([NotNull] ViGaSParser.InstanceDefinitionContext context)
        {
            var baseId = context.instanceHeader().baseId().GetText();
            var instanceId = context.instanceHeader().instanceId().GetText();

            currentInstance = World.Bases[baseId].Spawn(instanceId);

            World.Instances.Add(instanceId, currentInstance);

            var retVal = base.VisitInstanceDefinition(context);

            currentInstance = null;

            return retVal;
        }

        public object Visit(IParseTree parseTree, string script)
        {
            this.script = script;

            return Visit(parseTree);
        }
    }
}
