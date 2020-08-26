using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal
{
    public Dictionary<string, int> sgoal;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        sgoal = new Dictionary<string, int>();
        sgoal.Add(s, i);
        remove = r;
    }
}

public class GAgent : MonoBehaviour
{
    public string RestingTarget = "";
    public float rangeOfTarget = 0;
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public GInventory inventory = new GInventory();
    public WorldStates beliefs = new WorldStates();

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;

    Vector3 destination = Vector3.zero;

    protected void Start()
    {
        GAction[] acts = GetComponents<GAction>();
        foreach(GAction a in acts)
        {
            actions.Add(a);
        }
    }

    bool invoked = false;

    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.agent.isStopped = true;
        currentAction.agent.isStopped = false;
        currentAction.PostPerform();

        if (RestingTarget != "")
        {
            Vector3 restingPos = GameObject.FindGameObjectWithTag(RestingTarget).transform.position + new Vector3(
                Random.Range(-rangeOfTarget, rangeOfTarget),
                0,
                Random.Range(-rangeOfTarget, rangeOfTarget));

            currentAction.agent.SetDestination(restingPos);
        }
        
        invoked = false;
    }

    private void LateUpdate()
    {
        if (currentAction != null && currentAction.running)
        {
            float distanceToTarget = Vector3.Distance(destination, transform.position);
            if (currentAction.agent.hasPath && distanceToTarget < 2f)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }
        

        if (planner == null || actionQueue == null)
        {
            planner = new GPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach(KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.plan(actions, sg.Key.sgoal, beliefs);
                if (actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if (actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }

        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindGameObjectWithTag(currentAction.targetTag);
                }
                if (currentAction.target != null)
                {
                    currentAction.running = true;
                    destination = currentAction.target.transform.position + new Vector3(   Random.Range(-currentAction.rangeOfAction, currentAction.rangeOfAction),
                                                                                                0,
                                                                                                Random.Range(-currentAction.rangeOfAction, currentAction.rangeOfAction));

                    currentAction.agent.SetDestination(destination);
                }
            }
            else
            {
                actionQueue = null;
            }
        }

    }
}
