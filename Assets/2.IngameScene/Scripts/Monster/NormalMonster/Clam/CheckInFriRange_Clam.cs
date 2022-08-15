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
            //Debug.Log("[이민호] 주변에 몬스터 있음");
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;

    }
    
}
