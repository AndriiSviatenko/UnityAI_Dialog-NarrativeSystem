using UnityEngine;
using System;
using System.Collections.Generic;

namespace NarrativeSystem.Core
{
    [Serializable]
    public class Edge
    {
        public string TargetNodeGuid;
        public string PortID;
    }

    public abstract class BaseNode : ScriptableObject
    {
        [HideInInspector] public string Guid;
        [HideInInspector] public Vector2 EditorPosition;
        public List<Edge> Outputs = new List<Edge>();

        public virtual void OnCreate()
        {
            if (string.IsNullOrEmpty(Guid))
            {
                Guid = System.Guid.NewGuid().ToString();
            }
        }

        public abstract string GetNextNodeGuid();
    }

    [CreateAssetMenu(fileName = "New Dialogue Graph", menuName = "Narrative/Dialogue Graph")]
    public class DialogueGraph : ScriptableObject
    {
        public List<BaseNode> Nodes = new List<BaseNode>();
        public string EntryNodeGuid;

        public BaseNode GetNodeByGuid(string guid)
        {
            return Nodes.Find(n => n.Guid == guid);
        }

        public BaseNode GetEntryNode()
        {
            return GetNodeByGuid(EntryNodeGuid);
        }
    }
}
