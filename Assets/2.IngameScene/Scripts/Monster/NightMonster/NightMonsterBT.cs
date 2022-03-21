using System.Collections.Generic;

using BehaviorTree;

public class NightMonsterBT : BTTree
{
    public UnityEngine.Transform targetPoint;
    
    public static float speed = 2f;
    public static float attackRange = 0.7f;
    
    protected override Node SetUpTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckAttackRange(transform,targetPoint),
                new AtttackToTarget(transform, targetPoint)
            }),
            new FollowTarget(transform, targetPoint)
        });

        return root;
    }
}
