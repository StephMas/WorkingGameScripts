/*
 *
 *	Custom - Adventure Creator
 *	by Chris Burton, 2013-2016
 *	
 *	"ActionSendInteractionObject.cs"
 * 
 *	This action sends the object data to the controller for an animation event. Must be called prior to animation.
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
    public class ActionSendInteractionObject : Action
    {

        public SimpleInteractionObject intObject;
        public int objParameterID = -1;
        public int objConstantID = 0;

        public bool isHoldingObject;

        public Transform rightHoldPosRot;
        public int rHoldParameterID = -1;
        public int rHoldConstantID = 0;

        public Transform leftHoldPosRot;
        public int lHoldParameterID = -1;
        public int lHoldConstantID = 0;

        public Transform rightHandTarget;
        public Transform leftHandTarget;

        public SimpleInteractionSystem interactionController;
        public int controlParameterID = -1;
        public int controlConstantID = 0;
        public bool isPlayer;

        public Marker dropMarker;
        public int markerParameterID = -1;
        public int markerID = 0;

        public Char character;

        public int parameterID = -1;
        public int invID;
        private int invNumber;

        public GameObject objectPrefab;
        public int prefabConstantID = 0;
        public int prefabParameterID = -1;

        public enum InteractionType { TakeOut, PutAway, PickUp, Drop, Use };
        public InteractionType interactionType;

#if UNITY_EDITOR
        private InventoryManager inventoryManager;
        private SettingsManager settingsManager;
#endif



        public ActionSendInteractionObject()
        {
            this.isDisplayed = true;
            category = ActionCategory.Object;
            title = "Send Object Data";
            description = "Sends object data to the controller for an animation event. Must be called prior to animation.";
        }


        override public void AssignValues(List<ActionParameter> parameters)
        {
            intObject = AssignFile<SimpleInteractionObject>(parameters, objParameterID, objConstantID, intObject);
            objectPrefab = AssignFile(parameters, prefabParameterID, prefabConstantID, objectPrefab);
            rightHoldPosRot = AssignFile(parameters, rHoldParameterID, rHoldConstantID, rightHoldPosRot);
            leftHoldPosRot = AssignFile(parameters, lHoldParameterID, lHoldConstantID, leftHoldPosRot);
            invID = AssignInvItemID(parameters, parameterID, invID);

            if (!isPlayer)
            {
                interactionController = AssignFile<SimpleInteractionSystem>(parameters, controlParameterID, controlConstantID, interactionController);                
            }
        }


        override public float Run()
        {
            if (isPlayer)
            {
                interactionController = KickStarter.player.GetComponent<SimpleInteractionSystem>();

                if (interactionController == null)
                {
                    Debug.Log("There is no Object Interaction Control script assigned to this character!");
                }
            }
                       

            if (interactionType == InteractionType.PickUp || interactionType == InteractionType.Use)
            {
                if (interactionController.isHoldingObject == true)
                {
                    Debug.Log("You need to put down the object you're holding first.");
                }
                else
                {
                    interactionController.interactionObject = intObject;
                    interactionController.rightHoldPosition = rightHoldPosRot;
                    interactionController.leftHoldPosition = leftHoldPosRot;

                }

            }
            else if (interactionType == InteractionType.Drop)
            {
                if (interactionController.interactionObject == null || interactionController.isHoldingObject == false)
                {
                    Debug.Log("You aren't holding anything to drop.");
                }
                else
                {
                    interactionController.dropMarker = dropMarker;
                }
            }
            else if (interactionType == InteractionType.TakeOut)
            {
                objectPrefab = AC.KickStarter.runtimeInventory.GetItem(invID).linkedPrefab;

                if (interactionController.objectBeingHeld == true)
                {
                    Debug.Log("You are already holding an object.");
                }
                else
                {
                    interactionController.linkedPrefab = objectPrefab;                    
                    interactionController.rightHoldPosition = rightHoldPosRot;
                    interactionController.leftHoldPosition = leftHoldPosRot;

                }
            }
            return 0f;
        }


#if UNITY_EDITOR

        override public void ShowGUI(List<ActionParameter> parameters)
        {
            //Is this the player?
            isPlayer = EditorGUILayout.Toggle("Is Player?", isPlayer);
            if (!isPlayer)
            {
                controlParameterID = Action.ChooseParameterGUI("Character:", parameters, controlParameterID, ParameterType.GameObject);
                if (controlParameterID >= 0)
                {
                    controlConstantID = 0;
                    interactionController = null;
                }
                else
                {
                    interactionController = (SimpleInteractionSystem)EditorGUILayout.ObjectField("Character:", interactionController, typeof(SimpleInteractionSystem), true);

                    controlConstantID = FieldToID(interactionController, controlConstantID);
                    interactionController = IDToField(interactionController, controlConstantID, false);
                }

            }

            rightHandTarget = (Transform)EditorGUILayout.ObjectField("Right Hand Target:", rightHandTarget, typeof(Transform), true);
            leftHandTarget = (Transform)EditorGUILayout.ObjectField("Left Hand Target:", leftHandTarget, typeof(Transform), true);

            //What kind of interaction is going to happen?
            interactionType = (InteractionType)EditorGUILayout.EnumPopup("Interaction Type:", interactionType);

            //If you're going to be picking up the item, send the item to the interaction controller
            if (interactionType == InteractionType.PickUp || interactionType == InteractionType.Use)
            {
                objParameterID = Action.ChooseParameterGUI("Object to Pick Up:", parameters, objParameterID, ParameterType.GameObject);
                if (objParameterID >= 0)
                {
                    objConstantID = 0;
                    intObject = null;
                }
                else
                {
                    intObject = (SimpleInteractionObject)EditorGUILayout.ObjectField("Object to Pick Up:", intObject, typeof(SimpleInteractionObject), true);

                    objConstantID = FieldToID(intObject, objConstantID);
                    intObject = IDToField(intObject, objConstantID, false);
                }
            }
            if (interactionType == InteractionType.PickUp)
            {
                rHoldParameterID = Action.ChooseParameterGUI("Right Hold transform:", parameters, rHoldParameterID, ParameterType.GameObject);
                if (rHoldParameterID >= 0)
                {
                    rHoldConstantID = 0;
                    rightHoldPosRot = null;
                }
                else
                {
                    rightHoldPosRot = (Transform)EditorGUILayout.ObjectField("Right Hold transform:", rightHoldPosRot, typeof(Transform), true);

                    rHoldConstantID = FieldToID(rightHoldPosRot, rHoldConstantID);
                    rightHoldPosRot = IDToField(rightHoldPosRot, rHoldConstantID, false);
                }

                lHoldParameterID = Action.ChooseParameterGUI("Left Hold transform:", parameters, lHoldParameterID, ParameterType.GameObject);
                if (lHoldParameterID >= 0)
                {
                    lHoldConstantID = 0;
                    leftHoldPosRot = null;
                }
                else
                {
                    leftHoldPosRot = (Transform)EditorGUILayout.ObjectField("Left Hold transform:", leftHoldPosRot, typeof(Transform), true);

                    lHoldConstantID = FieldToID(leftHoldPosRot, lHoldConstantID);
                    leftHoldPosRot = IDToField(leftHoldPosRot, lHoldConstantID, false);
                }

            }

            //If you're going to drop the item, send the place you want to drop it to the interaction controller
            else if (interactionType == InteractionType.Drop)
            {
                markerParameterID = Action.ChooseParameterGUI("Drop at:", parameters, markerParameterID, ParameterType.GameObject);
                if (markerParameterID >= 0)
                {
                    markerID = 0;
                    dropMarker = null;
                }
                else
                {
                    dropMarker = (Marker)EditorGUILayout.ObjectField("Drop at:", dropMarker, typeof(Marker), true);

                    markerID = FieldToID<Marker>(dropMarker, markerID);
                    dropMarker = IDToField<Marker>(dropMarker, markerID, false);
                }
            }

            //If you're going to take out the item, select the item from your inventory
            else if (interactionType == InteractionType.TakeOut)
            {
                if (!inventoryManager)
                {
                    inventoryManager = AdvGame.GetReferences().inventoryManager;
                }
                if (!settingsManager)
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
                            ACDebug.LogWarning("Previously chosen item no longer exists!");
                            invNumber = 0;
                            invID = 0;
                        }

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

                        objectPrefab = AC.KickStarter.runtimeInventory.GetItem(invID).linkedPrefab;

                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No inventory items exist!", MessageType.Info);
                        invID = -1;
                        invNumber = -1;
                    }

                }
            }
            else if (interactionType == InteractionType.Use)
            {
                EditorGUILayout.HelpBox("This interaction will not be called from an animation event.", MessageType.Info);
            }

            AfterRunningOption();
        }


        override public void AssignConstantIDs(bool saveScriptsToo)
        {
            AssignConstantID<Marker>(dropMarker, markerID, markerParameterID);
        }


        override public string SetLabel()
        {
            string labelAdd = "";

            if (dropMarker)
            {
                if (intObject)
                {
                    labelAdd = " (" + intObject.name + " to " + dropMarker.name + ")";
                }

            }

            return labelAdd;
        }

#endif
    }

}