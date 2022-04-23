using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GuardFollowTarget : Node
{
    private Transform _transform;
    private Transform target_transform;
    private Animator _animator;
    
    
    public GuardFollowTarget(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        
        
        target_transform = (Transform) GetData("target");
        
        if (Vector3.Distance(_transform.position, target_transform.position) > 0.1f)
        {
            _animator.SetBool("Chasting", true);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Idle", false);
            _animator.SetBool("Hit", false);
            _transform.position = Vector3.MoveTowards(_transform.position, target_transform.position,
                SlimyeeBT.guardSpeed * Time.deltaTime);
            _transform.LookAt(target_transform.position);
            
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}
