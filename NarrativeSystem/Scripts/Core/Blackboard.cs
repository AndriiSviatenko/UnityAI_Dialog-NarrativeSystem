using UnityEngine;
using System;
using System.Collections.Generic;

namespace NarrativeSystem.Core
{
    [CreateAssetMenu(fileName = "New Blackboard", menuName = "Narrative/Blackboard")]
    public class Blackboard : ScriptableObject, IBlackboard
    {
        [Serializable]
        private class Entry<T>
        {
            public string Key;
            public T Value;
        }

        [SerializeField] private List<Entry<bool>> _bools = new List<Entry<bool>>();
        [SerializeField] private List<Entry<int>> _ints = new List<Entry<int>>();
        [SerializeField] private List<Entry<string>> _strings = new List<Entry<string>>();
        [SerializeField] private List<Entry<float>> _floats = new List<Entry<float>>();

        private Dictionary<string, bool> _runtimeBools = new Dictionary<string, bool>();
        private Dictionary<string, int> _runtimeInts = new Dictionary<string, int>();
        private Dictionary<string, string> _runtimeStrings = new Dictionary<string, string>();
        private Dictionary<string, float> _runtimeFloats = new Dictionary<string, float>();

        public event Action<string> OnValueChanged;

        private void OnEnable()
        {
            InitializeRuntime();
        }

        public void InitializeRuntime()
        {
            _runtimeBools.Clear();
            foreach (var e in _bools) _runtimeBools[e.Key] = e.Value;
            
            _runtimeInts.Clear();
            foreach (var e in _ints) _runtimeInts[e.Key] = e.Value;
            
            _runtimeStrings.Clear();
            foreach (var e in _strings) _runtimeStrings[e.Key] = e.Value;
            
            _runtimeFloats.Clear();
            foreach (var e in _floats) _runtimeFloats[e.Key] = e.Value;
        }

        public bool GetBool(string key, bool defaultValue = false) => _runtimeBools.TryGetValue(key, out var val) ? val : defaultValue;
        public void SetBool(string key, bool value) { _runtimeBools[key] = value; OnValueChanged?.Invoke(key); }

        public int GetInt(string key, int defaultValue = 0) => _runtimeInts.TryGetValue(key, out var val) ? val : defaultValue;
        public void SetInt(string key, int value) { _runtimeInts[key] = value; OnValueChanged?.Invoke(key); }

        public string GetString(string key, string defaultValue = "") => _runtimeStrings.TryGetValue(key, out var val) ? val : defaultValue;
        public void SetString(string key, string value) { _runtimeStrings[key] = value; OnValueChanged?.Invoke(key); }

        public float GetFloat(string key, float defaultValue = 0f) => _runtimeFloats.TryGetValue(key, out var val) ? val : defaultValue;
        public void SetFloat(string key, float value) { _runtimeFloats[key] = value; OnValueChanged?.Invoke(key); }

        public bool HasKey(string key) => _runtimeBools.ContainsKey(key) || _runtimeInts.ContainsKey(key) || _runtimeStrings.ContainsKey(key) || _runtimeFloats.ContainsKey(key);

        public void Clear()
        {
            _runtimeBools.Clear();
            _runtimeInts.Clear();
            _runtimeStrings.Clear();
            _runtimeFloats.Clear();
        }
    }
}
