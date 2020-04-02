using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using GameScript.Models;
using GameScript.Models.BaseClasses;
using GameScript.Models.InstanceClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Visitors
{
    public class WorldBuilderVisitor : ViGaSBaseVisitor<object>
    {
        private World world;
        private GameObject currentGameObject;
        private GameObjectInstance currentGameObjectInstance;

        public WorldBuilderVisitor()
        {
            world = new World();
        }

        public override object Visit([NotNull] IParseTree tree)
        {
            return base.Visit(tree);
        }

        public override object VisitBaseDefinition([NotNull] ViGaSParser.BaseDefinitionContext context)
        {
            //factory
            var baseClass = context.baseHeader().baseClass().GetText();


            return base.VisitBaseDefinition(context);
        }

        public override object VisitInstanceDefinition([NotNull] ViGaSParser.InstanceDefinitionContext context)
        {
            return base.VisitInstanceDefinition(context);
        }
    }
}
