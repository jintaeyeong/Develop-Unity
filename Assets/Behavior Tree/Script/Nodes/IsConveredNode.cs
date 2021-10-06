using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsConveredNode : Node
{
    private Transform target;
    private Transform origin;

    public IsConveredNode(Transform target, Transform origin)
    {
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;
        if(Physics.Raycast(origin.position, target.position - origin.position, out hit))
        {
            if(hit.collider.transform != target)
            {
                return NodeState.Success;
            }
        }
        return NodeState.Failure;
    }
}
