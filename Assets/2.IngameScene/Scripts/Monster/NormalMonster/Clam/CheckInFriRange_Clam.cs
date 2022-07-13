using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using BehaviorTree;

public class CheckInFriRange_Clam : Node
{
    private Transform _transform;

    public CheckInFriRange_Clam(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Monster"));

        object t = GetData("target");

        Collider[] colliders = Physics.OverlapSphere(_transform.position, ClamBT.socialityRange, layerMask);

        if (t != null && colliders.Length == 1)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;

    }
    
}
