using UnityEngine;
using System;

namespace NarrativeSystem.Nodes
{
    [Serializable]
    public class DialogueLine
    {
        public string SpeakerID;
        public string Text;
        public AudioClip VoiceOver;
        public string Expression;
    }

    [Serializable]
    public class ChoiceOption
    {
        public string Text;
        public string TargetNodeGuid;
    }
}
