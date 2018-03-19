/*
 *
 *	Custom - Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionInteractionPickUp.cs"
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
    public class ActionInteractionPickUp : Action
    {

        //The hand enums to match the system
        public enum HandsUsed { None, Both, Right, Left };
        public HandsUsed handsUsed;

        //The asset list to load
        public InteractionObjectList interactionList;

        //The object to choose from the list
        public InteractionObject interactionObject;
        public int objectID;
        public int objectNum;
        
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

        public ActionInteractionPickUp()
        {
            this.isDisplayed = true;
            category = ActionCategory.Object;
            title = "Pick Up";
            description = "Sends data for a Pick Up Interaction from an object to the system and triggers the chosen animation.";
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

        public void PopulateValues(InteractionSystem intSystem)
        {
            trigger = animator.parameters[animatorNum];

            //Load the correct interaction object list
            intSystem.interactionList = interactionList;

            //The data that needs to be stored
            intSystem.interactionObject = interactionObject;
            

            if (handsUsed == HandsUsed.Both)
            {
                intSystem.isBothHands = true;
                intSystem.isRightHand = false;
                intSystem.isLeftHand = false;
                intSystem.noHandIK = false;
            }
            if (handsUsed == HandsUsed.Right)
            {
                intSystem.isRightHand = true;
                intSystem.isLeftHand = false;
                intSystem.isBothHands = false;
                intSystem.noHandIK = false;
            }
            if (handsUsed == HandsUsed.Left)
            {
                intSystem.isLeftHand = true;
                intSystem.isBothHands = false;
                intSystem.isRightHand = false;
                intSystem.noHandIK = false;
            }
            if (handsUsed == HandsUsed.None)
            {
                intSystem.noHandIK = true;
                intSystem.isLeftHand = false;
                intSystem.isBothHands = false;
                intSystem.isRightHand = false;
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

            //Get the asset file
            interactionList = (InteractionObjectList)EditorGUILayout.ObjectField("Interaction Object List:", interactionList, typeof(InteractionObjectList), true);

            if (interactionList)
            {
                //Create a string list for the objects in the list
                List<string> objects = new List<string>();

                int index = 0;

                if (interactionList.objectList.Count > 0)
                {
                    foreach (InteractionObjectData datas in interactionList.objectList)
                    {
                        objects.Add(datas.myObject.objectName);
                        index++;
                    }

                    string label = "Object:";

                    objectNum = EditorGUILayout.Popup(label, objectNum, objects.ToArray());
                    interactionObject = interactionList.objectList[objectNum].myObject;
                    
                }
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


            AfterRunningOption();
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