/*
 *
 *	Custom - Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionSimplePickUp.cs"
 * 
 *	This action picks up an item.
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
    public class ActionSimplePickUp : Action
    {

        //The hand enums to match the system
        public enum HandsUsed { Both, Right, Left };
        public HandsUsed handsUsed;

        //The object
        public SimpleInteractionObject interactionObject;
        public int objectConstantID = 0;
        public int objectParameterID = -1;

        //The character
        public SimpleInteractionSystem interactionSystem;
        public bool isPlayer;
        public Char interactionCharacter;
        public int intCharConstantID = 0;
        public int intCharParameterID = -1;

        //The animation to run
        public Animator animator;
        public int animatorID;
        public int animatorNum;
        public AnimatorControllerParameter trigger;

        public ActionSimplePickUp()
        {
            this.isDisplayed = true;
            category = ActionCategory.Interaction;
            title = "Simple Pick Up";
            description = "Sends data for a Simple Pick Up Interaction from an object to the system and triggers the chosen animation.";
        }


        override public void AssignValues(List<ActionParameter> parameters)
        {
            interactionObject = AssignFile<SimpleInteractionObject>(parameters, objectParameterID, objectConstantID, interactionObject);
            interactionCharacter = AssignFile<Char>(parameters, intCharParameterID, intCharConstantID, interactionCharacter);

            if (isPlayer)
            {
                interactionCharacter = KickStarter.player;
            }
        }


        override public float Run()
        {
            interactionSystem = interactionCharacter.GetComponent<SimpleInteractionSystem>();
            animator = KickStarter.player.GetComponent<Animator>();

            if (interactionSystem != null)
            {
                //Clear the existing values
                interactionSystem.ClearData();

                //Send the new values
                PopulateValues(interactionSystem);
            }
            else
            {
                Debug.Log("There is no interaction system attached to this character.");
            }
            return 0f;
        }

        public void PopulateValues(SimpleInteractionSystem simpleIntSystem)
        {
            trigger = animator.parameters[animatorNum];
            //The object to pick up
            simpleIntSystem.interactionObject = interactionObject;

            if (interactionObject)
            {
                //The target data
                simpleIntSystem.emptyParent = interactionObject.emptyParent;

                //The drop position
                simpleIntSystem.dropMarker = interactionObject.dropPosition;
            }

            //The hands and target data used
            if (handsUsed == HandsUsed.Both)
            {
                simpleIntSystem.handsUsed = SimpleInteractionSystem.HandsUsed.BothHands;
                simpleIntSystem.rightHandTarget = interactionObject.rightHandTarget;
                simpleIntSystem.leftHandTarget = interactionObject.leftHandTarget;

                //The right hand hold position
                simpleIntSystem.rightHoldPosition = interactionObject.rightHoldPosition;
                //The left hand hold position
                simpleIntSystem.leftHoldPosition = interactionObject.leftHoldPosition;
            }
            else if (handsUsed == HandsUsed.Right)
            {
                simpleIntSystem.handsUsed = SimpleInteractionSystem.HandsUsed.RightHand;
                simpleIntSystem.rightHandTarget = interactionObject.rightHandTarget;
                simpleIntSystem.leftHandTarget = null;

                //The right hand hold position
                simpleIntSystem.rightHoldPosition = interactionObject.rightHoldPosition;
                //The left hand hold position can be set to null
                simpleIntSystem.leftHoldPosition = null;

            }
            else if (handsUsed == HandsUsed.Left)
            {
                simpleIntSystem.handsUsed = SimpleInteractionSystem.HandsUsed.LeftHand;
                simpleIntSystem.leftHandTarget = interactionObject.leftHandTarget;
                simpleIntSystem.rightHandTarget = null;

                //The right hand hold position can be set to null
                simpleIntSystem.rightHoldPosition = null;
                //The left hand hold position
                simpleIntSystem.leftHoldPosition = interactionObject.leftHoldPosition;
            }

            //The sounds if effects will be used
            if (interactionObject.holdSound != null)
            {
                simpleIntSystem.holdSound = interactionObject.holdSound;
            }
            if (interactionObject.dropSound != null)
            {
                simpleIntSystem.dropSound = interactionObject.dropSound;
            }
            if (interactionObject.interactionSound != null)
            {
                simpleIntSystem.interactionSound = interactionObject.interactionSound;
            }
            if (interactionObject.inventorySound != null)
            {
                simpleIntSystem.inventorySound = interactionObject.inventorySound;
            }


            InteractionAnimation(trigger.name);
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

        override public void ShowGUI (List<ActionParameter> parameters)
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

            objectParameterID = Action.ChooseParameterGUI("Interaction Object:", parameters, objectParameterID, ParameterType.GameObject);
            if (objectParameterID >= 0)
            {
                objectConstantID = 0;
                interactionObject = null;
            }
            else
            {
                interactionObject = (SimpleInteractionObject)EditorGUILayout.ObjectField("Interaction Object:", interactionObject, typeof(SimpleInteractionObject), true);

                objectConstantID = FieldToID(interactionObject, objectConstantID);
                interactionObject = IDToField(interactionObject, objectConstantID, false);
            }

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

            }


            AfterRunningOption ();
		}
		
		
		override public string SetLabel ()
		{
			string labelAdd = "";
			string labelItem = "";	
			
			labelAdd = " (" + labelItem + ")";
		
			return labelAdd;
		}

#endif

    }

}