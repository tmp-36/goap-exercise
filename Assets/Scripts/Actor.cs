using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Actor : MonoBehaviour
{
    public Goal[] goals;
    public Action[] actions;
    public Blackboard beliefs;
    public Blackboard worldState;

    [SerializeField] Transform lightSwitch;
    [SerializeField] Transform restPoint;

    public Plan plan;
    Action currentAction = null;

    [NonSerialized] public NavMeshAgent agent;

    private void OnValidate()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    private void Awake()
    {
        foreach (var action in actions)
        {
            action.Init(this);
        }
    }

    private void Start()
    {
        // agent.SetDestination(restPoint.position);
    }

    private void Update()
    {
        if (plan == null)
        {
            if (Planner.CreatePlan(this, goals, out plan))
            {
                currentAction = null;
            }
        }
        else
        {
            if (currentAction == null && plan.actions.Count > 0)
            {
                currentAction = plan.actions.Dequeue();
                currentAction.Start();
            }
            
            if (currentAction != null && currentAction.strategy.IsComplete())
            {
                currentAction.Stop();
                currentAction.Complete();
                currentAction = null;
            }

            if (plan.actions.Count == 0)
            {
                currentAction = null;
                plan = null;
            }
        }
    }

    public WorldState GetPlanningState()
    {
        WorldState result = new WorldState();
        result.values = beliefs.runtime.values | worldState.runtime.values;
        result.mask = beliefs.runtime.mask | worldState.runtime.mask;
        return result;
    }
}
