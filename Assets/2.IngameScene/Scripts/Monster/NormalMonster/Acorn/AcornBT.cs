using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AcornBT : BTTree
{
    public static float speed = 2f;
    public static float guardSpeed = 2.0f;
    public static float damageValue = 10.0f;
    public static float runAwaySpeed = 2.0f;
    
    public static float attackRange = 1.5f;
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
                    new CheckInFriRange_Acorn(transform),
                    new RunAwayTarget_Acorn(transform)
                }),
                // Guard Follow
                new Sequence(new List<Node>
                {
                    new CheckGuardTarget_Acorn(transform),
                    new GuardFollowTarget_Acorn(transform)
                })
            }),
            // Attack
            new Sequence(new List<Node>
            {
                new CheckAttackRange(transform),
                new AttackToTarget_Acorn(transform)
                }),
            // Follow
            new Sequence(new List<Node>
            {
                new CheckToPlayerInWater(transform),
                new CheckInFOVRange_Acorn(transform),
                new FollowTarget(transform)
            }),
            // Random Move
            new RandomMove_Acorn(transform)

        });

        return root;
    }
}
