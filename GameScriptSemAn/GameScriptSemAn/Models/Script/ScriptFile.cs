using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Models.Script
{
    public class ScriptFile
    {
        public string LongFileName { get; set; }
        public string ShortFileName { get; set; }
        public TextDocument Document { get; set; }

        public static ScriptFile CreateFromFile(string path)
        {
            var scriptFile = new ScriptFile();
            scriptFile.LongFileName = path;
            scriptFile.ShortFileName = path;
            scriptFile.Document = new TextDocument()
            {
                Text = File.ReadAllText(path)
            };

            return scriptFile;
        }
    }
}
