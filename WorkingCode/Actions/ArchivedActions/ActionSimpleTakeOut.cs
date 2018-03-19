/*
 *
 *	Custom - Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionSimpleTakeOut.cs"
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
    public class ActionSimpleTakeOut : Action
    {
        
        //The object, which will be a child of the linkedPrefab
        public SimpleInteractionObject interactionObject;
        public int objectConstantID = 0;
        public int objectParameterID = -1;

        //The character
        public SimpleInteractionSystem interactionSystem;
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

        public ActionSimpleTakeOut()
        {
            this.isDisplayed = true;
            category = ActionCategory.Interaction;
            title = "Simple Take Out";
            description = "Takes an object out of your bag -- selected from the inventory.";
        }


        override public void AssignValues(List<ActionParameter> parameters)
        {
            interactionObject = AssignFile<SimpleInteractionObject>(parameters, objectParameterID, objectConstantID, interactionObject);
            interactionCharacter = AssignFile<Char>(parameters, intCharParameterID, intCharConstantID, interactionCharacter);
            invID = AssignInvItemID(parameters, parameterID, invID);
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

            GameObject selectedParentItem = AC.KickStarter.inventoryManager.GetItem(invID).linkedPrefab;
            simpleIntSystem.linkedPrefab = selectedParentItem;

            SimpleInteractionObject intObject = selectedParentItem.GetComponentInChildren<SimpleInteractionObject>();
            simpleIntSystem.interactionObject = intObject;

            //The left hand hold position is the only one we need here
            simpleIntSystem.leftHoldPosition = intObject.leftHoldPosition;

            //The right hand hold position can be set to null
            simpleIntSystem.rightHoldPosition = null;

            //The drop position
            simpleIntSystem.dropMarker = intObject.dropPosition;

            //The sounds if effects will be used
            if (intObject.holdSound != null)
            {
                simpleIntSystem.holdSound = intObject.holdSound;
            }
            if (intObject.dropSound != null)
            {
                simpleIntSystem.dropSound = intObject.dropSound;
            }
            if (intObject.interactionSound != null)
            {
                simpleIntSystem.interactionSound = intObject.interactionSound;
            }

            if (intObject.inventorySound != null)
            {
                simpleIntSystem.inventorySound = intObject.inventorySound;
            }

            InteractionAnimation();
        }

        public void InteractionAnimation()
        {
            interactionCharacter.GetComponent<Animator>().SetTrigger("TakeOut");
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


            if (inventoryManager == null && AdvGame.GetReferences().inventoryManager)
            {
                inventoryManager = AdvGame.GetReferences().inventoryManager;
            }
            if (settingsManager == null && AdvGame.GetReferences().settingsManager)
            {
                settingsManager = AdvGame.GetReferences().settingsManager;
            }

            if (inventoryManager)
            {
                // Create a string List of the field's names (for the PopUp box)
                List<string> labelList = new List<string>();

                int i = 0;
                if (parameterID == -1)
                {
                    invNumber = -1;
                }

                if (inventoryManager.items.Count > 0)
                {

                    foreach (InvItem _item in inventoryManager.items)
                    {
                        labelList.Add(_item.label);

                        // If an item has been removed, make sure selected variable is still valid
                        if (_item.id == invID)
                        {
                            invNumber = i;
                        }

                        i++;
                    }

                    if (invNumber == -1)
                    {
                        // Wasn't found (item was possibly deleted), so revert to zero
                        ACDebug.LogWarning("Previously chosen item no longer exists!");

                        invNumber = 0;
                        invID = 0;
                    }

                    //
                    parameterID = Action.ChooseParameterGUI("Inventory item:", parameters, parameterID, ParameterType.InventoryItem);
                    if (parameterID >= 0)
                    {
                        invNumber = Mathf.Min(invNumber, inventoryManager.items.Count - 1);
                        invID = -1;
                    }
                    else
                    {
                        invNumber = EditorGUILayout.Popup("Inventory item:", invNumber, labelList.ToArray());
                        invID = inventoryManager.items[invNumber].id;
                    }
                    // 
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