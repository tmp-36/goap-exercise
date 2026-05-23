using System;
using System.Collections;

[Serializable]
public class Goal : IComparable<Goal>
{
    public float priority;
    public WorldState desiredState; 

    public int CompareTo(Goal other)
    {
        return priority.CompareTo(other.priority);
    }
}
