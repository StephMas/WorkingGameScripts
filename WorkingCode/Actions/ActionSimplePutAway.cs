/*
 *
 *	Custom - Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionSimplePutAway.cs"
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
    public class ActionSimplePutAway : Action
    {
      
        //Read the object that is currently being held
        public SimpleInteractionObject interactionObject;
        public int objectConstantID = 0;
        public int objectParameterID = -1;

        //The character
        public SimpleInteractionSystem interactionSystem;
        public bool isPlayer;
        public Char interactionCharacter;
        public int intCharConstantID = 0;
        public int intCharParameterID = -1;

        public ActionSimplePutAway()
        {
            this.isDisplayed = true;
            category = ActionCategory.Interaction;
            title = "Simple Put Away";
            description = "Takes an object out of your bag -- selected from the inventory.";
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

            if (interactionSystem != null)
            {
                if (interactionSystem.objectBeingHeld)
                {
                    if (interactionSystem.handsUsed == SimpleInteractionSystem.HandsUsed.LeftHand)
                    {
                        interactionCharacter.GetComponent<Animator>().SetTrigger("PutAwayLeft");
                        interactionSystem.ClearData();
                    }
                    if (interactionSystem.handsUsed == SimpleInteractionSystem.HandsUsed.RightHand)
                    {
                        interactionCharacter.GetComponent<Animator>().SetTrigger("PutAwayRight");
                        interactionSystem.ClearData();
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