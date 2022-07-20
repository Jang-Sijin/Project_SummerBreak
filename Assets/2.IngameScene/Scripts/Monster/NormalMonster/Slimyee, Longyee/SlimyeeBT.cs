using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class SlimyeeBT : BTTree
{
    public static float speed = 2f;
    public static float guardSpeed = 1.0f;
    public static float damageValue = 10.0f;
    public static float runAwaySpeed = 2.0f;
    
    public static float attackRange = 0.7f;
    public static float fovRange = 6.0f;
    public static float guardFovRange = 10.0f;
    public static float socialityRange = 20.0f;

    public bool guardCheck = false;
    public bool aloneCheck = false;
    public Vector3 randomTargetGoal = Vector3.zero;
    public bool isGrounded = false;
    public bool playerInWater = false;
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
            new Condition(new List<Node>
            {
                // Sociality
                new Sequence(new List<Node>
                {
                   new CheckInFRIRange(transform),
                   new RunAwayTarget(transform)
                }),
                // Guard Follow
                new Sequence(new List<Node>
                {
                    new CheckGuardTarget(transform),
                    new GuardFollowTarget(transform)
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
                new CheckToPlayerInWater(transform),
                new CheckInFOVRange(transform),
                new FollowTarget(transform)
            }),
            //Random Move
            new RandomMove(transform)
        });

        return root;
    }
}
