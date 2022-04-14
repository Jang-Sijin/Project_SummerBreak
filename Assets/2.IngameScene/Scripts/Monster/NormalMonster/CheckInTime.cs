using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckInTime : Node
{
    private int inTime;

    private MonsterManager _monsterManager;
    public CheckInTime(Transform transform)
    {
        _monsterManager = transform.GetComponent<MonsterManager>();
    }

    public override NodeState Evaluate()
    {
        inTime = GameManager.instance.GetInGameTime().Hour;
        if ((inTime < 5 && inTime >= 0) || (inTime >= 20 && inTime <= 23))
        {
            state = NodeState.SUCCESS;
            return state;
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}
