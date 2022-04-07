using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;


public class AtttackToTarget : Node
{
    private Transform _lastTarget;
    private Animator _animator;
    private PlayerMovement playerMovement;
    private Rigidbody _rigidbody;
    
    private float _attackTime = 0.5f;
    private float _attackCounter = 0f;
    
    public AtttackToTarget(Transform transform, Transform t_transform)
    {
        _lastTarget = t_transform;
        _animator = transform.GetComponent<Animator>();
        _rigidbody = transform.GetComponent<Rigidbody>();
        playerMovement = t_transform.GetComponent<PlayerMovement>();
    }

    public override NodeState Evaluate()
    {
        
        _attackCounter += Time.deltaTime;
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", true);
        if (_attackCounter >= _attackTime)
        {
            Debug.Log("공격함");
            playerMovement.HitStart(NightMonsterBT.damageValue,_rigidbody);
            _animator.SetBool("Attack", true);
            _animator.SetBool("Idle", false);
            bool targetIsDead = false;
            if (!targetIsDead)
            {
                _attackCounter = 0f;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
    
}
