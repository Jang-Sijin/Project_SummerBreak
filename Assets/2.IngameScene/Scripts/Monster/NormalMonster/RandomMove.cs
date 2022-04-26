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

    public RandomMove(Transform transform)
    {
        _transform = transform;
        _monsterManager = transform.GetComponent<MonsterManager>();
        _slimyeeBt = transform.GetComponent<SlimyeeBT>();
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        if (_slimyeeBt.randomTargetGoal == Vector3.zero)
        {
            _slimyeeBt.randomTargetGoal = _monsterManager.RandomPoint(SlimyeeBT.speed * 3);
        }
        else if (Vector3.Distance(_transform.position, _slimyeeBt.randomTargetGoal) < 0.1f)
        {
            _slimyeeBt.randomTargetGoal = _monsterManager.RandomPoint( SlimyeeBT.speed * 3);
        }
        
        _animator.SetBool("Chasting", true);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", false);
        _animator.SetBool("Hit", false);
        _transform.position = Vector3.MoveTowards(_transform.position, _slimyeeBt.randomTargetGoal,
            NightMonsterBT.speed * Time.deltaTime);
        _transform.LookAt(_slimyeeBt.randomTargetGoal);
        
        
        state = NodeState.RUNNING;
        return state;
    }
}
