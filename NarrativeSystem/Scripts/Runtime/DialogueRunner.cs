using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using NarrativeSystem.Core;
using NarrativeSystem.Nodes;

namespace NarrativeSystem.Runtime
{
    public interface IDialogueView
    {
        Task DisplayLine(DialogueLine line, CancellationToken ct);
        Task<int> DisplayChoices(List<ChoiceOption> choices, CancellationToken ct);
        void Close();
    }

    public class DialogueRunner
    {
        private DialogueGraph _currentGraph;
        private BaseNode _currentNode;
        private IBlackboard _blackboard;
        private IDialogueView _view;
        private Stack<string> _history = new Stack<string>();
        private CancellationTokenSource _loopCts;
        private bool _backRequested;
        private bool _isFastForwarding;

        public bool IsRunning { get; private set; }

        public DialogueRunner(IBlackboard blackboard, IDialogueView view)
        {
            _blackboard = blackboard;
            _view = view;
        }

        public async void StartDialogue(DialogueGraph graph)
        {
            if (IsRunning) return;
            if (graph == null) { Debug.LogError("DialogueRunner: Graph is null!"); return; }
            
            _currentGraph = graph;
            _currentNode = graph.GetEntryNode();
            _history.Clear();
            _backRequested = false;
            _isFastForwarding = false;
            
            if (_currentNode == null)
            {
                Debug.LogError($"DialogueRunner: Entry node not found for GUID: {graph.EntryNodeGuid}");
                return;
            }

            Debug.Log($"DialogueRunner: Starting graph {graph.name} at node {_currentNode.Guid}");
            IsRunning = true;

            try 
            {
                await RunLoop();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                IsRunning = false;
                _isFastForwarding = false;
                _view.Close();
                Debug.Log("DialogueRunner: Finished.");
            }
        }

        public void Skip()
        {
            if (!IsRunning) return;
            _loopCts?.Cancel();
        }

        public void FastForward(bool active)
        {
            _isFastForwarding = active;
            if (active && IsRunning)
            {
                _loopCts?.Cancel();
            }
        }

        public void GoToPreviousNode()
        {
            if (!IsRunning || _history.Count == 0) return;

            _backRequested = true;
            _isFastForwarding = false;
            _loopCts?.Cancel();
        }

        private async Task RunLoop()
        {
            while (_currentNode != null)
            {
                Debug.Log($"DialogueRunner: Processing node {_currentNode.GetType().Name} ({_currentNode.Guid})");
                _loopCts = new CancellationTokenSource();
                string currentGuid = _currentNode.Guid;

                try 
                {
                    if (_currentNode is LineNode lineNode)
                    {
                        if (!_isFastForwarding)
                        {
                            await _view.DisplayLine(lineNode.Line, _loopCts.Token);
                        }
                        
                        _history.Push(currentGuid);
                        string nextGuid = lineNode.GetNextNodeGuid();
                        _currentNode = _currentGraph.GetNodeByGuid(nextGuid);
                    }
                    else if (_currentNode is ChoiceNode choiceNode)
                    {
                        int index = 0;
                        if (!_isFastForwarding)
                        {
                            index = await _view.DisplayChoices(choiceNode.Choices, _loopCts.Token);
                        }
                        
                        _history.Push(currentGuid);
                        string nextGuid = choiceNode.Choices[index].TargetNodeGuid;
                        _currentNode = _currentGraph.GetNodeByGuid(nextGuid);
                    }
                    else if (_currentNode is ConditionNode conditionNode)
                    {
                        string nextGuid = conditionNode.GetNextNodeGuid(_blackboard);
                        _currentNode = _currentGraph.GetNodeByGuid(nextGuid);
                    }
                    else if (_currentNode is ActionNode actionNode)
                    {
                        actionNode.Execute(_blackboard);
                        string nextGuid = actionNode.GetNextNodeGuid();
                        _currentNode = _currentGraph.GetNodeByGuid(nextGuid);
                    }
                    else
                    {
                        Debug.Log("DialogueRunner: Reached unknown node type or end.");
                        break;
                    }
                }
                catch (OperationCanceledException)
                {
                    if (_backRequested)
                    {
                        _backRequested = false;
                        if (_history.Count > 0)
                        {
                            string prevGuid = _history.Pop();
                            _currentNode = _currentGraph.GetNodeByGuid(prevGuid);
                        }
                    }
                    else if (_isFastForwarding)
                    {
                        // Cancellation by FF trigger, loop will handle next node
                    }
                    else
                    {
                        if (_currentNode is LineNode lineNode)
                        {
                            _history.Push(currentGuid);
                            string nextGuid = lineNode.GetNextNodeGuid();
                            _currentNode = _currentGraph.GetNodeByGuid(nextGuid);
                        }
                    }
                }
            }
        }
}
}
