using System;
using UnityEngine;
using UnityEngine.AI;

public interface IActionStrategy
{
    public abstract void Init(Actor actor);
    public abstract void Start();
    public abstract void Update(float deltaTime);
    public abstract void Stop();
    public abstract bool CanPerform();
    public abstract bool IsComplete();
}

[Serializable]
public class Action
{
    public Blackboard blackboard;
    public float cost;
    public WorldState conditions;
    public WorldState effects;

    [SerializeReference, SubclassSelector]
    public IActionStrategy strategy;

    [NonSerialized] public Actor actor;

    public void Init(Actor actor)
    {
        this.actor = actor;
        strategy.Init(actor);
    }

    public void Start()
    {
        strategy.Start();
    }

    public void Stop()
    {
        strategy.Stop();
    }

    public void Update(float deltaTime)
    {
        strategy.Update(deltaTime);
    }

    public void Complete()
    {
        // TODO(Sergei): Which blackboard to apply this to
        WorldState world = blackboard.runtime;
        world.values = (world.values & ~effects.mask) | (effects.values & effects.mask);
        blackboard.runtime = world;
    }
}

[Serializable]
public class GotoPoint : IActionStrategy
{
    public Transform target;
    NavMeshAgent agent;
    Actor actor;

    public void Init(Actor actor)
    {
        this.actor = actor;
        agent = actor.agent;
    }

    public bool CanPerform()
    {
        return IsComplete() == false;
    }

    public bool IsComplete()
    {
        if ((agent.remainingDistance <= agent.stoppingDistance) && !agent.pathPending)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
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
    public Toggleable lightSwitch;
    public bool value;
    
    NavMeshAgent agent;

    public bool CanPerform()
    {
        return Vector3.Distance(agent.transform.position, lightSwitch.transform.position) <= 1.0f;
    }

    public void Init(Actor actor)
    {
        agent = actor.agent;
    }

    public bool IsComplete()
    {
        return true;
    }

    public void Start()
    {
        lightSwitch.SetActive(value);
    }

    public void Stop()
    {

    }

    public void Update(float deltaTime)
    {

    }
}
