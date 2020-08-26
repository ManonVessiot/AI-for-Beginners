using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class NodePlanner
{
    public NodePlanner parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public NodePlanner(NodePlanner _parent, float _cost, Dictionary<string, int> _states, GAction _action)
    {
        parent = _parent;
        cost = _cost;
        state = new Dictionary<string, int>(_states);
        action = _action;
    }

    public NodePlanner(NodePlanner _parent, float _cost, Dictionary<string, int> _states, Dictionary<string, int> _beliefstates, GAction _action)
    {
        parent = _parent;
        cost = _cost;
        state = new Dictionary<string, int>(_states);
        foreach(KeyValuePair<string, int> b in _beliefstates)
        {
            if (!state.ContainsKey(b.Key))
            {
                state.Add(b.Key, b.Value);
            }
        }
        action = _action;
    }
}

public class GPlanner
{
    public Queue<GAction> plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefstates)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach(GAction a in actions)
        {
            if (a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }
        List<NodePlanner> leaves = new List<NodePlanner>();
        NodePlanner start = new NodePlanner(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefstates.GetStates(), null);

        bool succes = BuildGraph(start, leaves, usableActions, goal);

        if (!succes)
        {
            //Debug.Log("NO PLAN");
            return null;
        }

        NodePlanner cheapest = null;
        foreach(NodePlanner leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else if (leaf.cost < cheapest.cost)
            {
                cheapest = leaf;
            }
        }

        List<GAction> result = new List<GAction>();
        NodePlanner n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach(GAction a in result)
        {
            queue.Enqueue(a);
        }
        /*
        Debug.Log("The Plan is : ");
        foreach (GAction a in queue)
        {
            Debug.Log("Q: " + a.actionName);
        }//*/

        return queue;
    }

    private bool BuildGraph(NodePlanner parent, List<NodePlanner> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach(GAction action in usableActions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(KeyValuePair<string, int> eff in action.effects)
                {
                    if (!currentState.ContainsKey(eff.Key))
                    {
                        currentState.Add(eff.Key, eff.Value);
                    }
                }

                NodePlanner node = new NodePlanner(parent, parent.cost + action.cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();
        foreach(GAction a in actions)
        {
            if (!a.Equals(removeMe))
            {
                subset.Add(a);
            }
        }
        return subset;
    }
}
