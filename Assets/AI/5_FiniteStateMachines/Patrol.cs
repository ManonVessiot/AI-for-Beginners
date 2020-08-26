using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    int currentIndex = -1;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL;
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < GameEnvironnement.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWP = GameEnvironnement.Singleton.Checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if (distance < lastDist)
            {
                currentIndex = i;
                lastDist = distance;
            }
        }
        currentIndex++;
        if (currentIndex >= GameEnvironnement.Singleton.Checkpoints.Count)
        {
            currentIndex = 0;
        }

        anim.SetTrigger("isWalking");
        agent.speed = 2;
        agent.isStopped = false;
        agent.SetDestination(GameEnvironnement.Singleton.Checkpoints[currentIndex].transform.position);
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            /*
            if (currentIndex >= GameEnvironnement.Singleton.Checkpoints.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            agent.SetDestination(GameEnvironnement.Singleton.Checkpoints[currentIndex].transform.position);
            */

            agent.isStopped = true;
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

        if (IsPlayerBehind())
        {
            nextState = new Safe(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}
