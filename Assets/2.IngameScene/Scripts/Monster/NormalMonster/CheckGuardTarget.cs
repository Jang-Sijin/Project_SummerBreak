using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckGuardTarget : Node
{
    private Transform _transform;
    private SlimyeeBT slimyeeBt;
    public CheckGuardTarget(Transform transform)
    {
        _transform = transform;
        slimyeeBt = transform.GetComponent<SlimyeeBT>();
    }

    public override NodeState Evaluate()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Player"));
        
        object t = GetData("target");
        if (slimyeeBt.guardCheck)
        {
            state = NodeState.FAILURE;
            return state;
        }
        else if (t != null && Physics.CheckSphere(_transform.position, SlimyeeBT.fovRange, layerMask))
        {
            slimyeeBt.guardCheck = true;
            Debug.Log("[이민호] 인지모드");
            state = NodeState.FAILURE;
            return state;
        }
        else if (t == null)
        {
            Collider[] collider = Physics.OverlapSphere(_transform.position, SlimyeeBT.guardFovRange, layerMask);

            if (collider.Length > 0)
            {
                parent.parent.SetData("target",collider[0].transform);
                state = NodeState.SUCCESS;
                return state;
            }
            state = NodeState.FAILURE;
            return state;
        }
        Debug.Log("[이민호] 경계모드");
        
        state = NodeState.SUCCESS;
        return state;
    }
}
