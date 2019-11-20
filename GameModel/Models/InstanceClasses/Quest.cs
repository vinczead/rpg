using System.Collections.Generic;

namespace GameModel.Models
{
    public class Quest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string,string> Variables { get; set; }
    }
}