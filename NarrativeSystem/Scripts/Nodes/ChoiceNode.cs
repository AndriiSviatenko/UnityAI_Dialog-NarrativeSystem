using UnityEngine;
using System.Collections.Generic;
using NarrativeSystem.Core;

namespace NarrativeSystem.Nodes
{
    public class ChoiceNode : BaseNode
    {
        public List<ChoiceOption> Choices = new List<ChoiceOption>();

        public override string GetNextNodeGuid()
        {
            return null; 
        }
    }
}
