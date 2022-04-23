using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RunAwayTarget : Node
{
    private Transform _transform;
    
    
    public RunAwayTarget(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        
        
        state = NodeState.FAILURE;
        return state;
    }
}
