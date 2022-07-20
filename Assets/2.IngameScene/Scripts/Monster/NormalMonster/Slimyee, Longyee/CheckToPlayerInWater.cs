using BehaviorTree;
using UnityEngine;

public class CheckToPlayerInWater : Node
{
    private PlayerMovement playerMovement;

    public CheckToPlayerInWater(Transform transform)
    {
    }

    public override NodeState Evaluate()
    {
        playerMovement = GameManager.instance.playerGameObject.GetComponent<PlayerMovement>();
        
        if (playerMovement.checkMonsterFollow)
        {
            state = NodeState.FAILURE;
            return state;
        }
        
        state = NodeState.SUCCESS;
        return state;
    }
}
