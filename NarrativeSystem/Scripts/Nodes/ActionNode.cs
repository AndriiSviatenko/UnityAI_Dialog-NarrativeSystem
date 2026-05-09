using UnityEngine;
using System.Collections.Generic;
using NarrativeSystem.Core;
using NarrativeSystem.Logic;

namespace NarrativeSystem.Nodes
{
    public class ActionNode : BaseNode
    {
        [SerializeField] private List<ScriptableAction> _actions = new List<ScriptableAction>();

        public void Initialize(List<ScriptableAction> actions)
        {
            _actions = actions;
        }

        public void Execute(IBlackboard bb)
        {
            foreach (var action in _actions)
            {
                if (action != null) action.Execute(bb);
            }
        }

        public override string GetNextNodeGuid()
        {
            return Outputs.Count > 0 ? Outputs[0].TargetNodeGuid : null;
        }
    }
}
