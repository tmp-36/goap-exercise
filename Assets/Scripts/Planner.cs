using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public static class Planner
{
    public static Plan CreatePlan(Actor actor, Goal[] goals)
    {
        Array.Sort(goals);

        foreach (Goal goal in goals)
        {
            bool satisfied =
                Goal.IsSatisfied(goal, actor.beliefs) &&
                Goal.IsSatisfied(goal, actor.worldState);

            if (satisfied) continue;

            Node targetNode = new(null, 0, goal.desiredState);

            

        }

        Plan plan = new Plan();
        plan.goal = goals[0];

        return null;
    }

    public static void BuildGraph(Node parent, Action[] actions)
    {

    }

    
}

public class Pathfinder
{
    public Dictionary<Node, Node> cameFrom = new();
    public Dictionary<Node, float> costSoFar = new();

    public bool FindPath(Node start, Node goal, Action[] actions)
    {
        PriorityQueue<Node, int> frontier = new();
        frontier.Enqueue(start, 0);

        foreach (Action action in actions)
        {

        }

        return false;
    }
}

public class Node
{
    public Node parent;
    public Action action;
    public HashSet<Property> conditions = new();
    public List<Node> leaves;
    public float cost;

    public Node(Node parent, float cost, Property[] conditions)
    {
        this.parent = parent;
        this.cost = cost;

        foreach (var condition in conditions)
        {
            this.conditions.Add(condition);
        }
    }
}

public class Plan
{
    public Goal goal;
    public Stack<Action> actions;
}
