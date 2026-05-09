using UnityEngine;
using NarrativeSystem.Core;

namespace NarrativeSystem.Runtime
{
    public class DemoController : MonoBehaviour
    {
        [SerializeField] private DialogueGraph _graph;
        [SerializeField] private Blackboard _blackboard;
        [SerializeField] private BasicDialogueUI _ui;

        public void Initialize(DialogueGraph graph, Blackboard blackboard, BasicDialogueUI ui)
        {
            _graph = graph;
            _blackboard = blackboard;
            _ui = ui;
        }

        public void Skip()
        {
            _runner?.Skip();
        }

        public void GoBack()
        {
            _runner?.GoToPreviousNode();
        }

        public void SetFastForward(bool active)
        {
            _runner?.FastForward(active);
        }

        private DialogueRunner _runner;

        private void Start()
        {
            Debug.Log($"DemoController: Start. Graph: {_graph != null}, BB: {_blackboard != null}, UI: {_ui != null}");
            if (_graph == null || _blackboard == null || _ui == null)
            {
                Debug.LogError("DemoController: Missing references!");
                return;
            }

            _blackboard.InitializeRuntime();
            _runner = new DialogueRunner(_blackboard, _ui);
            _runner.StartDialogue(_graph);
        }
}
}
