using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using AC;


public class CurveBehaviour : StateMachineBehaviour {


    public Char player;
    public FullBodyBipedIK ik;

    public float positionWeight;
    public float rotationWeight;   
    public Transform rightHandTarget;
    public Transform leftHandTarget;

    public InteractionSystem intSystem;


	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = AC.KickStarter.player;
        ik = player.GetComponent<FullBodyBipedIK>();
        intSystem = player.GetComponent<InteractionSystem>();


        rightHandTarget = player.GetComponent<InteractionSystem>().interactionObject.rHandTarget;
        leftHandTarget = player.GetComponent<InteractionSystem>().interactionObject.lHandTarget;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        positionWeight = animator.GetFloat("positionWeight");
        rotationWeight = animator.GetFloat("rotationWeight");

        if (intSystem.isRightHand)
        {
            if (intSystem.interactionObject.rHandTarget != null)
            {

                ik.solver.rightHandEffector.position = rightHandTarget.position;
                ik.solver.rightHandEffector.rotation = rightHandTarget.rotation;
                ik.solver.rightHandEffector.positionWeight = positionWeight;
                ik.solver.rightHandEffector.rotationWeight = rotationWeight;
            }
        }

        else if (intSystem.isLeftHand)
        {
            if (intSystem.interactionObject.lHandTarget != null)
            {
                ik.solver.leftHandEffector.position = leftHandTarget.position;
                ik.solver.leftHandEffector.rotation = leftHandTarget.rotation;
                ik.solver.leftHandEffector.positionWeight = positionWeight;
                ik.solver.leftHandEffector.rotationWeight = rotationWeight;
            }
        }

        else if (intSystem.isBothHands)
        {
            if (intSystem.interactionObject.rHandTarget != null)
            {

                ik.solver.rightHandEffector.position = rightHandTarget.position;
                ik.solver.rightHandEffector.rotation = rightHandTarget.rotation;
                ik.solver.rightHandEffector.positionWeight = positionWeight;
                ik.solver.rightHandEffector.rotationWeight = rotationWeight;
            }
            if (intSystem.interactionObject.lHandTarget != null)
            {
                ik.solver.leftHandEffector.position = leftHandTarget.position;
                ik.solver.leftHandEffector.rotation = leftHandTarget.rotation;
                ik.solver.leftHandEffector.positionWeight = positionWeight;
                ik.solver.leftHandEffector.rotationWeight = rotationWeight;
            }
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
