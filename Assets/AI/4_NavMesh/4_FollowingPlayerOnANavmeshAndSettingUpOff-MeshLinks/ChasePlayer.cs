using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour {

    GameObject player;
    NavMeshAgent agent;

    void Start() {

        player = GameObject.FindWithTag("Player");
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {

        agent.SetDestination(player.transform.position);
    }
}
