using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckInFRIRange : Node
{
    private Transform _transform;
    private SlimyeeBT _slimyeeBt;
    public CheckInFRIRange(Transform transform)
    {
        _transform = transform;
        _slimyeeBt = transform.GetComponent<SlimyeeBT>();
    }

    public override NodeState Evaluate()
    {
        int layerMaskPlayer = (1 << LayerMask.NameToLayer("Player"));
        object tP = GetData("target");
        
        if (tP == null)
        {
            Collider[] colliderP = Physics.OverlapSphere(_transform.position, SlimyeeBT.guardFovRange, layerMaskPlayer);

            if (colliderP.Length > 0)
            {
                parent.parent.SetData("target",colliderP[0].transform);
            }
            else
            {
                state = NodeState.FAILURE;
                return state;
            }
        }
        
        int layerMask = (1 << LayerMask.NameToLayer("Monster"));
        Collider[] collider = Physics.OverlapSphere(_transform.position, SlimyeeBT.socialityRange, layerMask);
        if (collider.Length > 1)
        {
            //Debug.Log("[이민호] 주변에 친구 있음");
            state = NodeState.FAILURE;
            return state;
        }

        //Debug.Log("[이민호] 주변에 친구 없음");
        state = NodeState.SUCCESS;
        return state;
    }
}
