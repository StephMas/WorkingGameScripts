/*
 *
 *	Custom - Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionInteractionTakeOut.cs"
 * 
 *	This action takes an item out of your bag.
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
    public class ActionInteractionTakeOut : Action
    {
        //The hand enums to match the system
        public enum HandsUsed { Right, Left };
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

        //Inventory item
        public int parameterID = -1;
        public int invID;
        private int invNumber;


#if UNITY_EDITOR
        private InventoryManager inventoryManager;
        private SettingsManager settingsManager;
#endif

        public ActionInteractionTakeOut()
        {
            this.isDisplayed = true;
            category = ActionCategory.Object;
            title = "Take Out";
            description = "Takes an object out of your bag -- selected from the inventory.";
        }


        override public void AssignValues(List<ActionParameter> parameters)
        {
            interactionCharacter = AssignFile<Char>(parameters, intCharParameterID, intCharConstantID, interactionCharacter);
            invID = AssignInvItemID(parameters, parameterID, invID);
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

            //Load the correct interaction object list
            intSystem.interactionList = interactionList;

            //The data that needs to be stored
            intSystem.interactionObject = interactionObject;


            InteractionAnimation();
        }

        public void InteractionAnimation()
        {
            if (handsUsed == HandsUsed.Left)
            {
                interactionCharacter.GetComponent<Animator>().SetTrigger("TakeOutLeft");
                interactionSystem.isLeftHand = true;
                interactionSystem.isRightHand = false;
                interactionSystem.isBothHands = false;
                interactionSystem.noHandIK = false;
            }
            else if (handsUsed == HandsUsed.Right)
            {
                interactionCharacter.GetComponent<Animator>().SetTrigger("TakeOutRight");
                interactionSystem.isLeftHand = false;
                interactionSystem.isRightHand = true;
                interactionSystem.isBothHands = false;
                interactionSystem.noHandIK = false;
            }
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