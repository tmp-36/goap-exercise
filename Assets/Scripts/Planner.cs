using System;
using System.Collections.Generic;
using Utils;

public static class Planner
{
    public static Plan CreatePlan(Actor actor, Goal[] goals)
    {
        Array.Sort(goals);

        foreach (Goal goal in goals)
        {
            WorldState currentState = actor.GetPlanningState();

            if (WorldState.IsCompatible(currentState, goal.desiredState) == false)
            {
                Pathfinder pathfinder = new Pathfinder();
                pathfinder.FindPath(goal.desiredState, currentState, actor.actions);
            }
        }

        Plan plan = new Plan();
        plan.goal = goals[0];

        return null;
    }    
}

public class Pathfinder
{
    public Dictionary<WorldState, WorldState> cameFrom = new();
    public Dictionary<WorldState, float> costSoFar = new();
    public Dictionary<WorldState, Action> actionMap = new();

    public static float Heuristic(WorldState a, WorldState b)
    {
        uint difference = (b.values ^ a.values) & b.mask;
        return WorldState.PopCount(difference);
    }

    public bool FindPath(WorldState start, WorldState goal, Action[] actions)
    {
        PriorityQueue<WorldState, float> frontier = new();
        frontier.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (WorldState.IsCompatible(current, goal))
            {
                return true;
            }

            foreach (Action action in actions)
            {
                if (WorldState.AdvancesGoal(current, action.effects))
                {
                    WorldState next = current;
                    next.mask &= ~action.effects.mask;
                    next.mask |= action.conditions.mask;
                    next.values = (next.values & ~action.conditions.mask) | action.conditions.values;

                    float newCost = costSoFar[current] + action.cost;

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        cameFrom[next] = current;
                        actionMap[next] = action;

                        float priority = newCost + Heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                    }
                }
            }
        }

        return false;
    }
}

public class Plan
{
    public Goal goal;
    public Stack<Action> actions;
}
