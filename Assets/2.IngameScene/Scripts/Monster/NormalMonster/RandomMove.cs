using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class RandomMove : Node
{
    private Transform _transform;
    private MonsterManager _monsterManager;
    private SlimyeeBT _slimyeeBt;
    private Animator _animator;

    
    private float _randomMoveTime = 3.0f;
    private float _randomMoveCounter = 0f;
    
    public RandomMove(Transform transform)
    {
        _transform = transform;
        _monsterManager = transform.GetComponent<MonsterManager>();
        _slimyeeBt = transform.GetComponent<SlimyeeBT>();
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        _randomMoveCounter += Time.deltaTime;
        _animator.SetBool("Chasting", true);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", false);
        _animator.SetBool("Hit", false);
        if (_randomMoveCounter >= _randomMoveTime)
        {
            _randomMoveCounter = 0.0f;
            _slimyeeBt.randomTargetGoal = _monsterManager.RandomPoint( SlimyeeBT.guardSpeed * 3);
        }

        _transform.position = Vector3.MoveTowards(_transform.position, _slimyeeBt.randomTargetGoal,
            SlimyeeBT.guardSpeed* Time.deltaTime);
        _transform.LookAt(_slimyeeBt.randomTargetGoal);
        Debug.Log("[이민호] 몬스터 랜덤이동");

        state = NodeState.RUNNING;
        return state;
    }
}