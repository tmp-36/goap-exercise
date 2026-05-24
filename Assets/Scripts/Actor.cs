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
    public Action currentAction;

    NavMeshAgent agent;

    private void OnValidate()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    private void Start()
    {
        agent.SetDestination(restPoint.position);
    }

    private void Update()
    {
        if (plan == null)
        {
            if (Planner.CreatePlan(this, goals, out Plan newPlan))
            {
                plan = newPlan;
            }
        }
        else
        {
            if (currentAction == null && plan.actions.Count > 0)
            {
                currentAction = plan.actions.Pop();
                currentAction.strategy.Start();
            }
            
            if (currentAction != null && currentAction.strategy.IsComplete())
            {
                currentAction.strategy.Stop();
                currentAction = null;
            }
        }
    }

    public WorldState GetPlanningState()
    {
        WorldState result = new WorldState();
        result.values = beliefs.properties.values | worldState.properties.values;
        result.mask = beliefs.properties.mask | worldState.properties.mask;
        return result;
    }
}
