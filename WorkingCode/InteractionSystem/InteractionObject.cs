using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using AC;

//This script is attached to an interaction object to hold all of the references for the scriptable object list. The object will also require a constantID to find it.

[System.Serializable]
public class InteractionObject : MonoBehaviour
{
    //The object name
    public string objectName = "New Object";

    //The interaction object GameObject
    public GameObject interactionObject = null;

    //The interaction object references
    public Transform objectParent;
    public Transform rHandPosition;
    public Transform lHandPosition;
    public Transform rHandTarget;
    public Transform lHandTarget;

    //The drop position
    public Transform dropPosition;

    //The audio files associated with the object
    public AudioClip pickUpSound;
    public AudioClip dropSound;
    public AudioClip interactSound;
    public AudioClip inventorySound;

    //The AC references for the object
    public int invID;
    private int invNumber;

    //The ConstantID references
    public int interactionConstantID = 0;
    public int parentID = 0;

    //Other references
    public bool isInventoryItem = false;

    [CustomEditor(typeof(InteractionObject))]
    public class InteractionObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            InteractionObject myTarget = (InteractionObject)target;

            GUILayout.Label("Interaction Object Setup", EditorStyles.largeLabel);

            myTarget.objectParent = (Transform)CustomGUILayout.ObjectField<Transform>("Object Parent:", myTarget.objectParent, true);

            myTarget.interactionObject = (GameObject)CustomGUILayout.ObjectField<GameObject>("Object:", myTarget.interactionObject, true);


            if (myTarget.interactionObject)
            {
                GUILayout.Label("Object Name", EditorStyles.boldLabel);

                myTarget.objectName = EditorGUILayout.TextField(myTarget.objectName);

                GUILayout.Label("Object Hand Positions", EditorStyles.boldLabel);

                myTarget.rHandPosition = EditorGUILayout.ObjectField("Right Hand Position:", myTarget.rHandPosition, typeof(Transform), true) as Transform;
                myTarget.lHandPosition = EditorGUILayout.ObjectField("Left Hand Position:", myTarget.lHandPosition, typeof(Transform), true) as Transform;
                myTarget.dropPosition = EditorGUILayout.ObjectField("Drop Position:", myTarget.dropPosition, typeof(Transform), true) as Transform;

                GUILayout.Label("Object Targets", EditorStyles.boldLabel);

                myTarget.rHandTarget = EditorGUILayout.ObjectField("Right Hand Target:", myTarget.rHandTarget, typeof(Transform), true) as Transform;
                myTarget.lHandTarget = EditorGUILayout.ObjectField("Left Hand Position:", myTarget.lHandTarget, typeof(Transform), true) as Transform;

                GUILayout.Label("Object Sound FX", EditorStyles.boldLabel);

                myTarget.pickUpSound = EditorGUILayout.ObjectField("PickUp Sound:", myTarget.pickUpSound, typeof(AudioClip), false) as AudioClip;
                myTarget.dropSound = EditorGUILayout.ObjectField("Drop Sound:", myTarget.dropSound, typeof(AudioClip), false) as AudioClip;
                myTarget.interactSound = EditorGUILayout.ObjectField("Interact Sound:", myTarget.interactSound, typeof(AudioClip), false) as AudioClip;
                myTarget.inventorySound = EditorGUILayout.ObjectField("Inventory Sound:", myTarget.inventorySound, typeof(AudioClip), false) as AudioClip;
            }

            GUILayout.Label("Object Inventory Reference", EditorStyles.boldLabel);

            myTarget.isInventoryItem = EditorGUILayout.Toggle("Is also inventory item?", myTarget.isInventoryItem);

            if (myTarget.isInventoryItem)
            {
                // Create a string List of the field's names (for the PopUp box)
                List<string> labelList = new List<string>();

                int i = 0;
                if (AC.KickStarter.inventoryManager.items.Count > 0)
                {
                    foreach (InvItem _item in AC.KickStarter.inventoryManager.items)
                    {
                        labelList.Add(_item.label);

                        // If an item has been removed, make sure selected variable is still valid
                        if (_item.id == myTarget.invID)
                        {
                            myTarget.invNumber = i;
                        }

                        i++;
                    }

                    if (myTarget.invNumber == -1)
                    {
                        // Wasn't found (item was possibly deleted), so revert to zero
                        ACDebug.LogWarning("Previously chosen item no longer exists!");

                        myTarget.invNumber = 0;
                        myTarget.invID = 0;
                    }

                    myTarget.invNumber = EditorGUILayout.Popup("Inventory item:", myTarget.invNumber, labelList.ToArray());
                    myTarget.invID = AC.KickStarter.inventoryManager.items[myTarget.invNumber].id;
                }
            }
        }
    }
}






