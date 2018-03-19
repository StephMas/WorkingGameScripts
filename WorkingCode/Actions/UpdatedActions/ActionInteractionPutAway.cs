/*
 *
 *	Custom - Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionInteractionPutAway.cs"
 * 
 *	This action puts an item in your bag.
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
    public class ActionInteractionPutAway : Action
    {

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

        public ActionInteractionPutAway()
        {
            this.isDisplayed = true;
            category = ActionCategory.Object;
            title = "Put Away";
            description = "Puts an object in your bag -- must be held.";
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

                if (interactionSystem.objectBeingHeld)
                {
                    if (interactionSystem.isLeftHand)
                    {
                        interactionCharacter.GetComponent<Animator>().SetTrigger("PutAwayLeft");
                    }
                    else if (interactionSystem.isRightHand)
                    {
                        interactionCharacter.GetComponent<Animator>().SetTrigger("PutAwayRight");
                    }

                }

            }
            else
            {
                Debug.Log("There is no interaction system attached to this character.");
            }
            return 0f;
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