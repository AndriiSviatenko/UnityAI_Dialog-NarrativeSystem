using UnityEngine;
using NarrativeSystem.Core;

namespace NarrativeSystem.Nodes
{
    public class LineNode : BaseNode
    {
        public DialogueLine Line;

        public override string GetNextNodeGuid()
        {
            return Outputs.Count > 0 ? Outputs[0].TargetNodeGuid : null;
        }
    }
}
