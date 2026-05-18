using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Actor : MonoBehaviour
{
    [SerializeField] Goal[] goals;
    [SerializeField] Action[] actions;
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
            Planner.CreatePlan(this, goals);
        }
    }
}
