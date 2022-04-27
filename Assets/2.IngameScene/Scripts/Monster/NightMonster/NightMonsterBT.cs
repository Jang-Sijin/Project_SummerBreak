using System.Collections.Generic;

using BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = BehaviorTree.Sequence;

public class NightMonsterBT : BTTree
{
    public static float speed = 2f;
    public static float attackRange = 0.7f;
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
