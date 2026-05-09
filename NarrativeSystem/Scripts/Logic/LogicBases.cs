using UnityEngine;
using NarrativeSystem.Core;

namespace NarrativeSystem.Logic
{
    public abstract class ScriptableCondition : ScriptableObject
    {
        public abstract bool Evaluate(IBlackboard blackboard);
    }

    public abstract class ScriptableAction : ScriptableObject
    {
        public abstract void Execute(IBlackboard blackboard);
    }
}
