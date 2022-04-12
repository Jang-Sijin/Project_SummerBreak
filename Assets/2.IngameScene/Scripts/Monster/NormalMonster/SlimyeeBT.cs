using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

public class SlimyeeBT : BTTree
{
    public UnityEngine.Transform targetPoint;
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
            }),
            // Time Check
            new Sequence(new List<Node>
            {
            }),
        });

        return root;
    }
}
