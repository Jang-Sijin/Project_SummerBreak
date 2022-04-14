using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckHitMonster : Node
{
    private Transform _transform;
    
    private MonsterManager monsterManager;
    
    public CheckHitMonster(Transform transform)
    {
        _transform = transform;
        monsterManager = transform.GetComponent<MonsterManager>();
    }

    public override NodeState Evaluate()
    {
        if (monsterManager.checkHit == true)
        {
            //Debug.Log("[이민호] 타격함");
            state = NodeState.SUCCESS;
            return state;
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}
