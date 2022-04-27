using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;


public class CheckInFOVRange : Node
{
    private Transform _transform;
    
    public CheckInFOVRange(Transform transform)
    {
        _transform = transform;
    }


    public override NodeState Evaluate()
    {
        object t = GetData("target");

        if (t == null)
        {
            int layerMask = (1 << LayerMask.NameToLayer("Player"));
            Collider[] collider = Physics.OverlapSphere(_transform.position, SlimyeeBT.fovRange, layerMask);

            if (collider.Length > 0)
            {
                parent.parent.SetData("target",collider[0].transform);
                state = NodeState.SUCCESS;
                return state;
            }
            state = NodeState.FAILURE;
            return state;
        }
        //Debug.Log("[이민호] 인지");
        state = NodeState.SUCCESS;
        return state;
    }

}
