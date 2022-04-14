using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

public class SlimyeeBT : BTTree
{
    public static float speed = 2f;
    public static float attackRange = 0.7f;
    public static float fovRange = 6.0f;
    public static float damageValue = 10.0f;
    
    
    protected override Node SetUpTree()
    {
        Node root = new Selector(new List<Node>
        {
            // Hit
            new Sequence(new List<Node>
            {
                new CheckHitMonster(transform),
                new HitForTarget(transform)
            }),
            // Time : Night
            new Sequence(new List<Node>
            {
                new CheckInTime(transform),
                new Sequence(new List<Node>
                {
                    
                })
                
            }),
            // Attack
            new Sequence(new List<Node>
            {
                new CheckAttackRange(transform),
                new AtttackToTarget(transform)
            }),
            // Follow
            new Sequence(new List<Node>
            {
                new CheckInFOVRange(transform),
                new FollowTarget(transform)
            })
        });

        return root;
    }
}
