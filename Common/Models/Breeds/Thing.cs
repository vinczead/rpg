using Antlr4.Runtime.Misc;
using Antlr4.StringTemplate;
using Common.Script.Utility;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Script.ViGaSParser;

namespace Common.Models
{
    public class Thing
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public SpriteModel Model { get; set; }
        public virtual System.Type InstanceType { get { return typeof(ThingInstance); } }
        public Dictionary<string, RunBlockContext> RunBlocks { get; set; } = new Dictionary<string, RunBlockContext>();
        public Dictionary<string, string> RunBlockTexts
        {
            get
            {
                return RunBlocks.Select(keyValue =>
                {
                    var runBlock = keyValue.Value;
                    var start = runBlock.start.StartIndex;
                    var stop = runBlock.stop.StopIndex;
                    var text = runBlock.start.InputStream.GetText(new Interval(start, stop));
                    return new KeyValuePair<string, string>(keyValue.Key, text);
                }).ToDictionary(p => p.Key, p => p.Value);
            }
        }
        public Dictionary<string, Symbol> Variables { get; set; } = new Dictionary<string, Symbol>();

        public virtual ThingInstance Spawn(string instanceId = null)
        {
            ThingInstance instance = Activator.CreateInstance(InstanceType) as ThingInstance;

            instance.Breed = this;
            instance.Id = instanceId ?? Guid.NewGuid().ToString();
            instance.IsIdGenerated = instanceId == null;
            foreach (var variable in Variables.Values)
            {
                instance.Variables.Add(variable.Name, new Symbol(variable));
            }

            return instance;
        }

        public string Serialize()
        {
            using var stream = typeof(FunctionManager).Assembly.GetManifestResourceStream("Common.Templates.stg");
            using var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            TemplateGroup templateGroup = new TemplateGroupString(content);
            var template = templateGroup.GetInstanceOf("breed");
            template.Add("b", this);
            return template.Render();
        }
    }
}
