using UnityEngine;
using BehaviorTree;

public class CheckGuardTarget_Acorn : Node
{
    private Transform _transform;
    private AcornBT acornBt;

    public CheckGuardTarget_Acorn(Transform transform)
    {
        _transform = transform;
        acornBt = transform.GetComponent<AcornBT>();
    }

    public override NodeState Evaluate()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Player"));

        object t = GetData("target");
        if (acornBt.guardCheck)
        {
            state = NodeState.FAILURE;
            return state;
        }
        else if (t != null && Physics.CheckSphere(_transform.position, AcornBT.fovRange, layerMask))
        {
            acornBt.guardCheck = true;
            state = NodeState.FAILURE;
            return state;
        }
        else if (t == null)
        {
            Collider[] collider = Physics.OverlapSphere(_transform.position, AcornBT.guardFovRange, layerMask);

            if (collider.Length > 0)
            {
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
