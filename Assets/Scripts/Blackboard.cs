using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Blackboard")]
public class Blackboard : ScriptableObject
{
    [SerializeField] private WorldState properties;
    [NonSerialized] public WorldState runtime;

    private void OnEnable()
    {
        runtime = properties;
    }
}
