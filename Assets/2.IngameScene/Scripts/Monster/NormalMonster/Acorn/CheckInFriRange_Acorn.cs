using UnityEngine;
using BehaviorTree;

public class CheckInFriRange_Acorn : Node
{
    private Transform _transform;

    public CheckInFriRange_Acorn(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Monster"));

        object t = GetData("target");

        Collider[] colliders = Physics.OverlapSphere(_transform.position, AcornBT.socialityRange, layerMask);

        if (t != null && colliders.Length == 1)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;

    }
}
