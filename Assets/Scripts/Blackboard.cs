using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(menuName = "ScriptableObjects/Blackboard")]
public class Blackboard : ScriptableObject
{
    public SerializedDictionary<string, bool> dictionary = new();

    public bool HasKey(string key)
    {
        return dictionary.ContainsKey(key);
    }

    public void SetValue(string key, bool value)
    {
        dictionary[key] = value;
    }

    public bool GetValue(string key)
    {
        return dictionary[key];
    }
}
