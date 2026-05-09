using UnityEngine;

namespace NarrativeSystem.Core
{
    [CreateAssetMenu(fileName = "DialogueSettings", menuName = "Narrative/Settings")]
    public class DialogueSettings : ScriptableObject
    {
        [Header("Features")]
        public bool SkipEnabled = true;
        public bool AutoAdvanceDefault = false;
        public bool PauseGameDuringDialogue = true;

        [Header("Typing")]
        public float CharactersPerSecond = 20f;
        
        [Header("UI Defaults")]
        public float ChoiceFadeDuration = 0.2f;

        private static DialogueSettings _instance;
        public static DialogueSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<DialogueSettings>("DialogueSettings");
                    if (_instance == null)
                    {
                        _instance = CreateInstance<DialogueSettings>();
                    }
                }
                return _instance;
            }
        }
    }
}
