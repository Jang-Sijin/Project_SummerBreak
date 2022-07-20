using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AttackToTarget_Clam : Node
{
    private Animator _animator;

    private Transform _transform;
    private float _attackTime = 2.0f;
    private float _attackCounter = 0.0f;
    public AttackToTarget_Clam(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        _attackCounter += Time.deltaTime;
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", true);
        
        Transform target_transform = (Transform) GetData("target");
        
        _transform.LookAt(target_transform.position);
        if (_attackCounter >= _attackTime)
        {
            _animator.SetBool("Attack", true);
            _animator.SetBool("Idle", false);

            _attackCounter = 0.0f;
        }
        
        state = NodeState.RUNNING;
        return state;
    }
    
}
