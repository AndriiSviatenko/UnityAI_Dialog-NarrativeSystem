using UnityEngine;
using NarrativeSystem.Core;
using NarrativeSystem.Logic;

namespace NarrativeSystem.Nodes
{
    public class ConditionNode : BaseNode
    {
        [SerializeField] private ScriptableCondition _condition;
        
        public string TrueTargetGuid;
        public string FalseTargetGuid;

        public void Initialize(ScriptableCondition condition, string trueGuid, string falseGuid)
        {
            _condition = condition;
            TrueTargetGuid = trueGuid;
            FalseTargetGuid = falseGuid;
        }

        public bool Evaluate(IBlackboard bb)
        {
            return _condition != null && _condition.Evaluate(bb);
        }

        public override string GetNextNodeGuid() => null;

        public string GetNextNodeGuid(IBlackboard bb)
        {
            return Evaluate(bb) ? TrueTargetGuid : FalseTargetGuid;
        }
    }
}
