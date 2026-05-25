using System;
using System.Collections.Generic;
using Utils;

public static class Planner
{
    public static bool CreatePlan(Actor actor, Goal[] goals, out Plan plan)
    {
        Array.Sort(goals);

        foreach (Goal goal in goals)
        {
            WorldState currentState = actor.GetPlanningState();
            //goal.mask = start.mask;
            currentState.mask = goal.desiredState.mask;

            if (WorldState.IsCompatible(currentState, goal.desiredState) == false)
            {
                Pathfinder pathfinder = new Pathfinder();
                return pathfinder.FindPath(goal.desiredState, currentState, actor.actions, out plan);
            }
        }

        plan = null;
        return false;
    }    
}

public class Pathfinder
{
    public Dictionary<WorldState, WorldState> cameFrom = new();
    public Dictionary<WorldState, float> costSoFar = new();
    public Dictionary<WorldState, Action> actionMap = new();

    public static float Heuristic(WorldState a, WorldState b)
    {
        WorldFlags difference = (b.values ^ a.values) & b.mask;
        return WorldState.PopCount((uint)difference);
    }

    public bool FindPath(WorldState start, WorldState goal, Action[] actions, out Plan plan)
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
                plan = MakePlan(start, current);
                return true;
            }

            foreach (Action action in actions)
            {
                if (WorldState.AdvancesGoal(current, action.effects))
                {
                    goal.mask |= action.conditions.mask;

                    WorldState next = current;
                    next.mask &= ~action.effects.mask;
                    next.values &= next.mask;
                    next.mask |= action.conditions.mask;
                    next.values = (next.values & ~action.conditions.mask) | (action.conditions.values & action.conditions.mask);

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

        plan = null;
        return false;
    }

    public Plan MakePlan(WorldState start, WorldState goal)
    {
        Plan plan = new Plan();

        WorldState current = goal;

        while (!current.Equals(start))
        {
            if (actionMap.ContainsKey(current))
            {
                plan.actions.Enqueue(actionMap[current]);
            }

            current = cameFrom[current];
        }

        return plan;
    }
}

public class Plan
{
    public Queue<Action> actions = new();
}
