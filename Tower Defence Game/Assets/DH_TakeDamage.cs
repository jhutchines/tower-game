using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DH_TakeDamage : StateMachineBehaviour
{

    public GameObject enemyTarget;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyTarget = animator.gameObject.transform.parent.GetComponent<JH_UnitAttack>().damagedEnemy;
        if (enemyTarget.GetComponent<JH_Unit>().in_health > 0)
        {

            enemyTarget.GetComponent<JH_Unit>().animator.Play("Wound");        
        }
        else
        {
            enemyTarget.GetComponent<JH_Unit>().animator.Play("Death");
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
