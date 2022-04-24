using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckInTime : Node
{
    private int inTime;

    public CheckInTime() { }

    public override NodeState Evaluate()
    {
        inTime = GameManager.instance.GetInGameTime().Hour;
        if ((inTime < 5 && inTime >= 0) || (inTime >= 20 && inTime <= 23))
        {
            Debug.Log("[이민호] 밤임");
            state = NodeState.SUCCESS;
            return state;
        }
        Debug.Log("[이민호] 아침임");
        state = NodeState.FAILURE;
        return state;
        
    }
}
