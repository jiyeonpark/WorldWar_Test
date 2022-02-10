﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKStateMachine : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerInput.Instance.CamHead == null) return;
        //if (animator.GetBool("IK") == false) return;
        AISoldier ai = animator.GetComponent<AISoldier>();
        if (ai)
        {
            animator.SetLookAtPosition(PlayerInput.Instance.CamHead.position + Define.WCVector3(0f, -2f, 0f) + animator.transform.right * 0.5f);
            animator.SetLookAtWeight(1f, 1f, 0.8f);
        }

        //Debug.Log("IK");
        PlayerAvatar avatar = animator.GetComponent<PlayerAvatar>();
        if(avatar)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftHand, avatar.headBone.position + avatar.lHandPos);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, avatar.headBone.position + avatar.rHandPos);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        }
    }
}
