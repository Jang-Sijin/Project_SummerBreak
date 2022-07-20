using UnityEngine;
using BehaviorTree;

public class RunAwayTarget_Acorn : Node
{
    private Transform _transform;
    private Animator _animator;

    public RunAwayTarget_Acorn(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Transform player = (Transform) GetData("target");
        Vector3 dirToPlayer = _transform.position - player.position;
        Vector3 newPos = _transform.position + dirToPlayer;
        
        _transform.position = Vector3.MoveTowards(_transform.position, newPos,
            AcornBT.runAwaySpeed * Time.deltaTime);
        _transform.LookAt(newPos);
        
        _animator.SetBool("Chasting", true);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", false);
        _animator.SetBool("Hit", false);
        
        state = NodeState.RUNNING;
        return state;
    }
}
