using UnityEngine;
using NarrativeSystem.Core;

namespace NarrativeSystem.Logic
{
    [CreateAssetMenu(fileName = "IntCondition", menuName = "Narrative/Logic/Int Condition")]
    public class IntCondition : ScriptableCondition
    {
        public string Key;
        public enum Comparison { Equal, Greater, Less }
        public Comparison Op;
        public int Threshold;

        public override bool Evaluate(IBlackboard blackboard)
        {
            int val = blackboard.GetInt(Key);
            return Op switch
            {
                Comparison.Equal => val == Threshold,
                Comparison.Greater => val > Threshold,
                Comparison.Less => val < Threshold,
                _ => false
            };
        }
    }
}
