using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class RunAwayTarget : Node
{
    private Transform _transform;
    private Animator _animator;
    public RunAwayTarget(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("[이민호] 도망 노드에 들어옴");
        Transform player = (Transform)GetData("target");
        Vector3 dirToPlayer = _transform.position - player.position;
        Vector3 newPos = _transform.position + dirToPlayer;

        _transform.position = Vector3.MoveTowards(_transform.position, newPos,
            SlimyeeBT.runAwaySpeed * Time.deltaTime);
        _transform.LookAt(newPos);
        
        _animator.SetBool("Chasting", true);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", false);
        _animator.SetBool("Hit", false);
        
        state = NodeState.RUNNING;
        return state;
    }
}
