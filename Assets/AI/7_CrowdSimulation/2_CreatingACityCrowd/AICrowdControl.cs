using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICrowdControl : MonoBehaviour {

	GameObject[] goalLocations;
	NavMeshAgent agent;
    Animator anim;
    
	void Start () {
		goalLocations = GameObject.FindGameObjectsWithTag("goal");
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(goalLocations[Random.Range(0,goalLocations.Length)].transform.position);

        anim = this.GetComponent<Animator>();
        anim.SetFloat("wOffset", Random.Range(0, 1));
        anim.SetTrigger("isWalking");
        float speedMult = Random.Range(0.2f, 1.3f);
        agent.speed *= speedMult;
        anim.SetFloat("speedMult", speedMult); 
	}

    private void Update()
    {
        if (agent.remainingDistance < 2)
        {
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }
    }
}
