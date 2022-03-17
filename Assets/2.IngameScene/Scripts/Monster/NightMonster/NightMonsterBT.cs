using System.Collections.Generic;
using BehaviorTree;

public class NightMonsterBT : Tree
{
    public UnityEngine.Transform targetPoint;
    
    public static float speed = 2f;
    public static float attackRange = 1f;
    
    protected override Node SetUpTree()
    {
        Node root = new Selector(new List<Node>
        {
            
            new FollowTarget(transform, targetPoint)
        });

        return root;
    }
}
