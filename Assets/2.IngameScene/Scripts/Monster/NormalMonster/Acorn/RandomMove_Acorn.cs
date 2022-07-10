using UnityEngine;
using BehaviorTree;

public class RandomMove_Acorn : Node
{
    private Transform _transform;
    private MonsterManager _monsterManager;
    private AcornBT _acornBt;
    private Animator _animator;

    private float _randomMoveTime = 3.0f;
    private float _randomMoveCounter = 0.0f;

    public RandomMove_Acorn(Transform transform)
    {
        _transform = transform;
        _monsterManager = transform.GetComponent<MonsterManager>();
        _acornBt = transform.GetComponent<AcornBT>();
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        if (_acornBt.isGrounded == false)
        {
            _acornBt.isGrounded = Physics.Raycast(_transform.position, -Vector3.up, 0.2f);
        }
        else
        {
            _randomMoveCounter += Time.deltaTime;
            _animator.SetBool("Chasting", true);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Idle", false);
            _animator.SetBool("Hit", false);
            if (_randomMoveCounter >= _randomMoveTime)
            {
                _randomMoveCounter = 0.0f;
                _acornBt.randomTargetGoal =
                    _monsterManager.RandomPoint(SlimyeeBT.guardSpeed * 5) - _transform.position;
                _acornBt.randomTargetGoal.y = 0.0f;
                _acornBt.randomTargetGoal = _acornBt.randomTargetGoal.normalized;
            }

            Rigidbody rigidbody = _transform.GetComponent<Rigidbody>();
            rigidbody.velocity = _acornBt.randomTargetGoal * SlimyeeBT.guardSpeed;
            Quaternion rot = Quaternion.LookRotation(_acornBt.randomTargetGoal);

            _transform.rotation = rot;
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}
