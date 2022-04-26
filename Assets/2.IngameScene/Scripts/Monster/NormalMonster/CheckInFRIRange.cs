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
        if (_slimyeeBt.aloneCheck)
        {
            state = NodeState.SUCCESS;
            return state;
        }
        int layerMask = (1 << LayerMask.NameToLayer("Monster"));
        Collider[] collider = Physics.OverlapSphere(_transform.position, SlimyeeBT.socialityRange, layerMask);
        object t = GetData("target");
        if (collider.Length > 1 || t == null)
        {
            //Debug.Log("[이민호] 주변에 친구 있음");
            state = NodeState.FAILURE;
            return state;
        }

        _slimyeeBt.aloneCheck = true;
        //Debug.Log("[이민호] 주변에 친구 없음");
        state = NodeState.SUCCESS;
        return state;
    }
}
