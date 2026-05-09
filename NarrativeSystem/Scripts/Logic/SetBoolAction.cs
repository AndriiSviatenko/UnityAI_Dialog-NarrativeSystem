using UnityEngine;
using NarrativeSystem.Core;

namespace NarrativeSystem.Logic
{
    [CreateAssetMenu(fileName = "SetBoolAction", menuName = "Narrative/Logic/Set Bool Action")]
    public class SetBoolAction : ScriptableAction
    {
        public string Key;
        public bool Value = true;

        public override void Execute(IBlackboard blackboard)
        {
            blackboard.SetBool(Key, Value);
        }
    }
}
