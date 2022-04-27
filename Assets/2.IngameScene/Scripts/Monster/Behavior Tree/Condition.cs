using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;


namespace BehaviorTree
{
    public class Condition : Node
    {
        public Condition() : base() { }

        public Condition(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            NodeState checkState = new CheckInTime().Evaluate();
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
