using UnityEngine;
using BehaviorTree;


public class CheckInFOVRange_Clam : Node
{
    private Transform _transform;

    public CheckInFOVRange_Clam(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        if (t == null)
        {
            int layerMask = (1 << LayerMask.NameToLayer("Player"));
            Collider[] collider = Physics.OverlapSphere(_transform.position, ClamBT.fovRange, layerMask);

            if (collider.Length > 0)
            {
                //Debug.Log("[이민호] 플레이어 찾음");
                parent.parent.SetData("target",collider[0].transform);
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
    
}
