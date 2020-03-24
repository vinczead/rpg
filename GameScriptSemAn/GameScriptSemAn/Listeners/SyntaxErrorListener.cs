using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using GameScript.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScript.Listeners
{
    public class SyntaxErrorListener : BaseErrorListener
    {
        public List<Error> errors = new List<Error>();

        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            errors.Add(new Error(line, charPositionInLine, msg));
            base.SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        }
    }
}
