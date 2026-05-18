using UnityEngine;

public class ActionStrategy
{
    public bool isComplete;
}

public class Action
{
    public float cost;
    public Property[] conditions;
    public Property[] effects;

    public void Start()
    {

    }

    public void Update(float deltaTime)
    {

    }

    public void Stop()
    {

    }

    public bool CanPerform()
    {
        return true;
    }

    public void Execute()
    {

    }
}

public class GotoPoint : Action
{

}

public class ActivateObject : Action
{

}
