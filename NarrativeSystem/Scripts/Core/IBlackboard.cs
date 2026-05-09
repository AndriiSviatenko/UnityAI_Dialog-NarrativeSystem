using System;

namespace NarrativeSystem.Core
{
    public interface IBlackboard
    {
        bool GetBool(string key, bool defaultValue = false);
        void SetBool(string key, bool value);
        
        int GetInt(string key, int defaultValue = 0);
        void SetInt(string key, int value);
        
        string GetString(string key, string defaultValue = "");
        void SetString(string key, string value);
        
        float GetFloat(string key, float defaultValue = 0f);
        void SetFloat(string key, float value);

        bool HasKey(string key);
        void Clear();
        
        event Action<string> OnValueChanged;
    }
}
