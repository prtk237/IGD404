using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool isAnyChildRunning = false;

            foreach(Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;

                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        continue;

                    case NodeState.RUNNING:
                        isAnyChildRunning = true;
                        continue;

                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }

            state = isAnyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }

}

