using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class FollowTarget : Node
{
    private Transform m_transform;
    private Transform target_transform;
    private Animator _animator;
    
    public FollowTarget(Transform _transform)
    {
        m_transform = _transform;
        _animator = _transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {

        target_transform = (Transform) GetData("target");
        
        if (Vector3.Distance(m_transform.position, target_transform.position) > 0.1f)
        {
            _animator.SetBool("Chasting", true);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Idle", false);
            _animator.SetBool("Hit", false);
            m_transform.position = Vector3.MoveTowards(m_transform.position, target_transform.position,
                NightMonsterBT.speed * Time.deltaTime);
            m_transform.LookAt(target_transform.position);
            
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}
