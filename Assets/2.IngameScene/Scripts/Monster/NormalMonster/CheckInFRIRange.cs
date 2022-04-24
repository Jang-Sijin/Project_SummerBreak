using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckInFRIRange : Node
{
    private Transform _transform;
    
    public CheckInFRIRange(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Monster"));
        if (Physics.CheckSphere(_transform.position,SlimyeeBT.socialityRange, layerMask))
        {
            //Debug.Log("[이민호] 주변에 친구 있음");
            state = NodeState.FAILURE;
            return state;
        }
        
        state = NodeState.SUCCESS;
        return state;
    }
}
