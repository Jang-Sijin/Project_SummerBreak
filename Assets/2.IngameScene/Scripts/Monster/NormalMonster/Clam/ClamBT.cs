using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ClamBT : BTTree
{
    
    public static float damageValue = 10.0f;
    
    public static float attackRange = 10.0f;
    public static float fovRange = 6.0f;
    public static float guardFovRange = 10.0f;
    public static float socialityRange = 20.0f;
    
    public bool guardCheck = false;

    protected override Node SetUpTree()
    {
        Node root = new Selector(new List<Node>
        {
            //Hit
            new Sequence(new List<Node>
            {
                new CheckHitMonster(transform),
                new HitForTarget(transform)
            }),
            //Time : Night
            new Condition(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckInFriRange_Clam(transform)
                })
            }),
            // Attack
            new Sequence(new List<Node>
            {
                new CheckAttackRange(transform),
                new AttackToTarget_Clam(transform)
            }),
            // FOV
            new Sequence(new List<Node>
            {
                new CheckInFOVRange_Clam(transform)
            })
        });


        return root;
    }
}
