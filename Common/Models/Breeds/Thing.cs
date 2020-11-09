using Antlr4.Runtime.Misc;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        public virtual Type InstanceType { get { return typeof(ThingInstance); } }
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
        public virtual ThingInstance Spawn(string instanceId = null)
        {
            ThingInstance instance = Activator.CreateInstance(InstanceType) as ThingInstance;

            instance.Breed = this;
            instance.Id = instanceId ?? Guid.NewGuid().ToString();
            instance.IsIdGenerated = instanceId == null;
            return instance;
        }
    }
}
