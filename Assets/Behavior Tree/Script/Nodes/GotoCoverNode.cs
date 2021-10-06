using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GotoCoverNode : Node
{
    private NavMeshAgent agent;
    private EnemyAI ai;

    public GotoCoverNode(NavMeshAgent agent, EnemyAI ai)
    {
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Transform coverSpot = ai.GetBestCover();
        if(coverSpot == null)
        {
            return NodeState.Failure;
        }       
        float distance = Vector3.Distance(coverSpot.position, agent.transform.position);
        if (distance > 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(coverSpot.position);
            return NodeState.Running;
        }
        else
        {
            agent.isStopped = true;
            return NodeState.Success;
        }
    }

}
