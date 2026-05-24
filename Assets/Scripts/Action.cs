using System;
using UnityEngine;
using UnityEngine.AI;

public interface IActionStrategy
{
    public abstract void Start();
    public abstract void Update(float deltaTime);
    public abstract void Stop();
    public abstract bool CanPerform();
    public abstract bool IsComplete();
}

[Serializable]
public class Action
{
    public float cost;
    public WorldState conditions;
    public WorldState effects;

    [SerializeReference, SubclassSelector]
    public IActionStrategy strategy;
}

[Serializable]
public class GotoPoint : IActionStrategy
{
    public NavMeshAgent agent;
    public Transform target;

    public bool CanPerform()
    {
        return IsComplete() == false;
    }

    public bool IsComplete()
    {
        return agent.remainingDistance <= 2.0f && !agent.pathPending;
    }

    public void Start()
    {
        agent.SetDestination(target.position);
    }

    public void Stop()
    {
        agent.ResetPath();
    }

    public void Update(float deltaTime)
    {

    }
}

[Serializable]
public class ActivateObject : IActionStrategy
{
    public NavMeshAgent agent;
    public Toggleable lightSwitch;

    public bool CanPerform()
    {
        return Vector3.Distance(agent.transform.position, lightSwitch.transform.position) <= 2.5f;
    }

    public bool IsComplete()
    {
        return true;
    }

    public void Start()
    {
        lightSwitch.SetActive(true);
    }

    public void Stop()
    {

    }

    public void Update(float deltaTime)
    {

    }
}
