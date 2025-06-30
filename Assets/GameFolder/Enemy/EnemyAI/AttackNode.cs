using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
using Unity.VisualScripting;
public class AttackNode : Node
{
    //private static int _enemyLayer = 1 << 6;

    private bool isAttacking;
    private float attackTimer = 0f;
    private float attackDuration = 3f;

    private FloatVariable _value;
    

    public AttackNode(FloatVariable value)
    {
        _value = value;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (!isAttacking)
        {
            Debug.Log("Attack started!");
            //GuardBT.HP -= 10;
            _value.Value -= 10;
            isAttacking = true;
            attackTimer = attackDuration;
        }

        // Decrement the attack timer.
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            state = NodeState.RUNNING;
            return state;
        }
        else
        {
            // Attack finished.
            Debug.Log("Attack executed after " + attackDuration + " seconds.");
            state = NodeState.SUCCESS;
            isAttacking = false;  // Reset for the next attack (if desired).
            return state;
        }
    }

    
}
