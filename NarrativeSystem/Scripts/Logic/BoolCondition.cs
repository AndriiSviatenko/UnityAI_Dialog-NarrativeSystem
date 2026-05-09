using UnityEngine;
using NarrativeSystem.Core;

namespace NarrativeSystem.Logic
{
    [CreateAssetMenu(fileName = "BoolCondition", menuName = "Narrative/Logic/Bool Condition")]
    public class BoolCondition : ScriptableCondition
    {
        public string Key;
        public bool RequiredValue = true;

        public override bool Evaluate(IBlackboard blackboard)
        {
            return blackboard.GetBool(Key) == RequiredValue;
        }
    }
}
