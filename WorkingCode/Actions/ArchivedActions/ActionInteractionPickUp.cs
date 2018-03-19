/*
 *
 *	Custom - Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionInteractionPickUp.cs"
 * 
 *	This action prepares the interaction system.
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
        //The fields for the action
        public enum ObjectType { SceneObject, InventoryObject};
        public ObjectType objectType;

        //The hand enums to match the system
        public enum HandsUsed { Both, Right, Left};
        public HandsUsed handsUsed;

        //The result object, whether it's a prefab or an object
        public GameObject resultObject;

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

        //Inventory item
        public int parameterID = -1;
        public int invID;
        private int invNumber;

#if UNITY_EDITOR
        private InventoryManager inventoryManager;
        private SettingsManager settingsManager;
#endif

        public ActionInteractionPickUp()
        {
            this.isDisplayed = true;
            category = ActionCategory.Interaction;
            title = "Send Data";
            description = "Sends data for a Simple Interaction from an object to the system.";
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
            //The object data
            if (objectType == ObjectType.SceneObject)
            {
                simpleIntSystem.interactionObject = interactionObject;

                //The target data
                simpleIntSystem.emptyParent = interactionObject.emptyParent;

                //The drop position
                simpleIntSystem.dropMarker = interactionObject.dropPosition;

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
            }
            else
            {
                GameObject selectedItem = AC.KickStarter.inventoryManager.GetItem(invID).linkedPrefab;
                simpleIntSystem.linkedPrefab = selectedItem;

                SimpleInteractionObject selectedIntItem = selectedItem.GetComponent<SimpleInteractionObject>();

                simpleIntSystem.interactionObject = selectedIntItem;

                //The drop position
                simpleIntSystem.dropMarker = selectedIntItem.dropPosition;

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
                if (selectedIntItem.holdSound != null)
                {
                    simpleIntSystem.holdSound = selectedIntItem.holdSound;
                }
                if (selectedIntItem.dropSound != null)
                {
                    simpleIntSystem.dropSound = selectedIntItem.dropSound;
                }
                if (selectedIntItem.interactionSound != null)
                {
                    simpleIntSystem.interactionSound = selectedIntItem.interactionSound;
                }
            }




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

            handsUsed = (HandsUsed)EditorGUILayout.EnumPopup("Which hand(s) to send to interaction", handsUsed);

            objectType = (ObjectType)EditorGUILayout.EnumPopup("Object Type:", objectType);

            if (objectType == ObjectType.SceneObject)
            {
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
            }
            else if (objectType == ObjectType.InventoryObject)
            {
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