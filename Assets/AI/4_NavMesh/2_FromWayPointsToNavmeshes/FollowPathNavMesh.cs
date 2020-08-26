using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPathNavMesh : MonoBehaviour
{
    public GameObject wpManager;
    GameObject[] wps;
    NavMeshAgent agent;

    private void Start()
    {
        wps = wpManager.GetComponent<WPManager>().wayPoints;
        agent = GetComponent<NavMeshAgent>();
    }

    public void GoToPoint(int pt)
    {
        agent.SetDestination(wps[pt].transform.position);
    }
}

