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
        int layerMask = (1 << LayerMask.NameToLayer("Monster"));
        
        object t = GetData("target");
        Collider[] collider = Physics.OverlapSphere(_transform.position, SlimyeeBT.socialityRange, layerMask);
        if (t != null && collider.Length == 1)
        {
            state = NodeState.SUCCESS;
            return state;
            //Debug.Log("[이민호] 주변에 친구 없음");
        }

        //Debug.Log("[이민호] 주변에 친구 있음");
        state = NodeState.FAILURE;
        return state;
    }
}
