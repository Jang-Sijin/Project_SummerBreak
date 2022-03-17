using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class FollowTarget : Node
{
    private Transform m_transform;
    private Transform target_transform;
    
    public FollowTarget(Transform _transform, Transform t_transform)
    {
        m_transform = _transform;
        target_transform = t_transform;
    }

    public override NodeState Evaluate()
    {
        if (Vector3.Distance(m_transform.position, target_transform.position) > 1.25f)
        {
            m_transform.position = Vector3.MoveTowards(m_transform.position, target_transform.position,
                NightMonsterBT.speed * Time.deltaTime);
            m_transform.LookAt(target_transform.position);
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}
