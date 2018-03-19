/*
 *
 *	Custom - Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionInteractionDrop.cs"
 * 
 *	This action puts an item down in the scene.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

    [System.Serializable]
    public class ActionInteractionDrop : Action
    {
        //The hand enums to match the system
        public enum HandsUsed { None, Both, Right, Left };
        public HandsUsed handsUsed;

        //Read the object that is currently being held
        public InteractionObject interactionObject;
        public int objectConstantID = 0;
        public int objectParameterID = -1;

        //The character
        public InteractionSystem interactionSystem;
        public bool isPlayer;
        public Char interactionCharacter;
        public int intCharConstantID = 0;
        public int intCharParameterID = -1;

        //The animation to run
        public Animator animator;
        public int animatorID;
        public int animatorNum;
        public AnimatorControllerParameter trigger;

        public ActionInteractionDrop()
        {
            this.isDisplayed = true;
            category = ActionCategory.Object;
            title = "Drop";
            description = "Puts an object that you are holding back down.";
        }


        override public void AssignValues(List<ActionParameter> parameters)
        {
            interactionCharacter = AssignFile<Char>(parameters, intCharParameterID, intCharConstantID, interactionCharacter);

            if (isPlayer)
            {
                interactionCharacter = KickStarter.player;
            }
        }


        override public float Run()
        {
            interactionSystem = interactionCharacter.GetComponent<InteractionSystem>();

            if (interactionSystem != null)
            {
                if (handsUsed == HandsUsed.Both)
                {
                    interactionSystem.isBothHands = true;
                    interactionSystem.isRightHand = false;
                    interactionSystem.isLeftHand = false;
                    interactionSystem.noHandIK = false;
                }
                if (handsUsed == HandsUsed.Right)
                {
                    interactionSystem.isRightHand = true;
                    interactionSystem.isLeftHand = false;
                    interactionSystem.isBothHands = false;
                    interactionSystem.noHandIK = false;
                }
                if (handsUsed == HandsUsed.Left)
                {
                    interactionSystem.isLeftHand = true;
                    interactionSystem.isBothHands = false;
                    interactionSystem.isRightHand = false;
                    interactionSystem.noHandIK = false;
                }
                if (handsUsed == HandsUsed.None)
                {
                    interactionSystem.noHandIK = true;
                    interactionSystem.isLeftHand = false;
                    interactionSystem.isBothHands = false;
                    interactionSystem.isRightHand = false;
                }

                if (interactionSystem.objectBeingHeld)
                {
                    InteractionAnimation(trigger.name);
                }

            }
            else
            {
                Debug.Log("There is no interaction system attached to this character.");
            }
            return 0f;
        }

        public void InteractionAnimation(string triggerName)
        {
            //Just in case it skips the assignment
            if (animator == null)
            {
                animator = KickStarter.player.GetComponent<Animator>();
            }
            animator.SetTrigger(triggerName);
        }


#if UNITY_EDITOR

        override public void ShowGUI(List<ActionParameter> parameters)
        {
            isPlayer = EditorGUILayout.Toggle("Is Player?", isPlayer);

            if (!isPlayer)
            {
                intCharParameterID = Action.ChooseParameterGUI("Character:", parameters, intCharParameterID, ParameterType.GameObject);
                if (intCharParameterID >= 0)
                {
                    intCharConstantID = 0;
                    interactionCharacter = null;
                }
                else
                {
                    interactionCharacter = (Char)EditorGUILayout.ObjectField("Character:", interactionCharacter, typeof(Char), true);

                    intCharConstantID = FieldToID(interactionCharacter, intCharConstantID);
                    interactionCharacter = IDToField(interactionCharacter, intCharConstantID, false);
                }
            }
            else
            {
                interactionCharacter = AC.KickStarter.player;
            }

            handsUsed = (HandsUsed)EditorGUILayout.EnumPopup("Which hand(s) to use", handsUsed);

            if (interactionCharacter != null)
            {
                animator = interactionCharacter.GetComponent<Animator>();
                //Create a string list for the trigger parameters
                List<string> triggerList = new List<string>();

                int i = 0;

                if (animator.parameterCount > 0)
                {
                    foreach (AnimatorControllerParameter parameter in animator.parameters)
                    {
                        triggerList.Add(parameter.name);

                        //If the trigger has been removed for some reason, make sure the variable is still valid
                        if (parameter.nameHash == animatorID)
                        {
                            animatorNum = i;
                        }

                        i++;
                    }

                    if (animatorNum == -1)
                    {
                        Debug.Log("Previously chosen trigger parameter no longer exists!");
                        animatorNum = 0;
                        animatorID = 0;
                    }

                    string label = "Trigger:";

                    animatorNum = EditorGUILayout.Popup(label, animatorNum, triggerList.ToArray());
                    animatorID = animator.parameters[animatorNum].nameHash;
                    trigger = animator.parameters[animatorNum];
                }

                AfterRunningOption();
            }
        }


        override public string SetLabel()
        {
            string labelAdd = "";
            string labelItem = "";

            labelAdd = " (" + labelItem + ")";

            return labelAdd;
        }

#endif

    }

}