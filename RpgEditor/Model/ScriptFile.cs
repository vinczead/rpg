using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgEditor
{
    public class ScriptFile
    {
        public string LongFileName { get; set; }
        public string ShortFileName { get; set; }
        public TextDocument Document { get; set; }
    }
}
