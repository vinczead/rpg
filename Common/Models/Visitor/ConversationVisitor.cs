using Common.Script.Utility;
using Common.Script.Visitors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class ConversationVisitor : Visitor
    {
        private readonly CharacterInstance initiator;
        private readonly string topicId;
        public ConversationVisitor(CharacterInstance initiator, string topicId)
        {
            this.initiator = initiator;
            this.topicId = topicId;
        }

        public override void Visit(CharacterInstance subjectCharacter)
        {
            var initiatorSymbol = new Symbol("Character", TypeSystem.Instance["CharacterInstance"], initiator.Id);
            var topicSymbol = new Symbol("Topic", TypeSystem.Instance["String"], topicId);
            ExecutionVisitor.ExecuteRunBlock(subjectCharacter, "TalkedTo", new List<Symbol>() { initiatorSymbol, topicSymbol });
            success = true;
        }
    }
}
