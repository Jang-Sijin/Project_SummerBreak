using BehaviorTree;
using UnityEngine;

public class CheckToPlayerInWater : Node
{
    private PlayerMovement playerMovement;
    private SlimyeeBT slimyeeBt;

    public CheckToPlayerInWater(Transform transform)
    {
        slimyeeBt = transform.GetComponent<SlimyeeBT>();
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
