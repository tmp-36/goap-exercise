using System;

[Serializable]
public class Goal : IComparable<Goal>
{
    public float priority;
    public Property[] desiredState;

    public int CompareTo(Goal other)
    {
        return priority.CompareTo(other.priority);
    }

    public static bool IsSatisfied(Goal goal, Blackboard blackboard)
    {
        foreach (var state in goal.desiredState)
        {
            if (blackboard.HasKey(state.key))
            {
                if (state.value != blackboard.GetValue(state.key))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
