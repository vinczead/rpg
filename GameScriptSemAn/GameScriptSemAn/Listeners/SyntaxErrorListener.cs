using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
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
        /*
        public override void ReportAmbiguity([NotNull] Parser recognizer, [NotNull] DFA dfa, int startIndex, int stopIndex, bool exact, [Nullable] BitSet ambigAlts, [NotNull] ATNConfigSet configs)
        {
            errors.Add(new Error(1, 1, "ReportAmbiguity"));
            base.ReportAmbiguity(recognizer, dfa, startIndex, stopIndex, exact, ambigAlts, configs);
        }

        public override void ReportAttemptingFullContext([NotNull] Parser recognizer, [NotNull] DFA dfa, int startIndex, int stopIndex, [Nullable] BitSet conflictingAlts, [NotNull] SimulatorState conflictState)
        {
            errors.Add(new Error(1, 1, "ReportAttemptingFullContext"));
            base.ReportAttemptingFullContext(recognizer, dfa, startIndex, stopIndex, conflictingAlts, conflictState);
        }

        public override void ReportContextSensitivity([NotNull] Parser recognizer, [NotNull] DFA dfa, int startIndex, int stopIndex, int prediction, [NotNull] SimulatorState acceptState)
        {
            errors.Add(new Error(1, 1, "ReportContextSensitivity"));
            base.ReportContextSensitivity(recognizer, dfa, startIndex, stopIndex, prediction, acceptState);
        }*/
    }
}
