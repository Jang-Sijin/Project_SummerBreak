using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace  BehaviorTree
{
    public class Condition_CheckAttacking : Node
    {
        private Transform _transform;
        
        public Condition_CheckAttacking() : base() { }

        public Condition_CheckAttacking(List<Node> children, Transform transform) : base(children)
        {
            Debug.Log("[이민호] 계속 불리는 중");
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            NodeState checkState = new ChecktoAttacking(_transform).Evaluate();
            
            if (checkState == NodeState.SUCCESS)
            {
                foreach (Node node in children)
                {
                    switch (node.Evaluate())
                    {
                        case NodeState.FAILURE:
                            continue;
                        case NodeState.RUNNING:
                            state = NodeState.RUNNING;
                            return state;
                        case NodeState.SUCCESS:
                            state = NodeState.SUCCESS;
                            return state;
                        default:
                            continue;
                    }
                }
            }
            
            state = NodeState.FAILURE;
            return state;
        }
    }
}
