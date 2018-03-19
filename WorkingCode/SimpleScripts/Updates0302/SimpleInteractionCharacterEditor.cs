using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleInteractionSystem))]
public class SimpleInteractionCharacterEditor : Editor
{
    public bool handToggleGroupEnabled = true;

    public override void OnInspectorGUI()
    {
        SimpleInteractionSystem myTarget = (SimpleInteractionSystem)target;

        GUILayout.Space(20);

        myTarget.isMainCharacter = EditorGUILayout.Toggle("Main Character?", myTarget.isMainCharacter, EditorStyles.radioButton);
        if (myTarget.isMainCharacter == true)
        {
            EditorGUILayout.LabelField("SETTINGS APPLIED FOR MAIN CHARACTER", EditorStyles.whiteMiniLabel);
        }
        else
        {
            EditorGUILayout.LabelField("SETTINGS APPLIED FOR NPC CHARACTER", EditorStyles.whiteMiniLabel);
        }

        GUILayout.Space(10);

        if (myTarget.objectBeingHeld == true)
        {
            EditorGUILayout.ObjectField("You are currently holding:", myTarget.interactionObject, typeof(GameObject), true);
        }
        else
        {
            EditorGUILayout.LabelField("You are not currently holding anything.", EditorStyles.helpBox);
        }

        if (myTarget.toPlay != null)
        {
            EditorGUILayout.ObjectField("The interaction sound assigned is:", myTarget.toPlay, typeof(AudioClip), true);
        }

        GUILayout.Space(10);

        if (myTarget.interactionObject == null)
        {
            EditorGUILayout.LabelField("THERE IS NO INTERACTION OBJECT SET.", EditorStyles.miniBoldLabel);
        }
        else
        {
            EditorGUILayout.LabelField("OBJECT INFORMATION", EditorStyles.miniBoldLabel);

            GUILayout.Space(10);

            EditorGUILayout.BeginToggleGroup("Hand(s) Used:", handToggleGroupEnabled);
            myTarget.noHandIK = EditorGUILayout.ToggleLeft("No Hand IK Used", myTarget.noHandIK);
            myTarget.isRightHand = EditorGUILayout.ToggleLeft("Right Hand", myTarget.isRightHand);
            myTarget.isLeftHand = EditorGUILayout.ToggleLeft("Left Hand", myTarget.isLeftHand);
            myTarget.isBothHands = EditorGUILayout.ToggleLeft("Both Hands", myTarget.isBothHands);
            EditorGUILayout.EndToggleGroup();
        }
 
            if (myTarget.objectBeingHeld == true)
            {
                EditorGUILayout.LabelField("THIS OBJECT IS CURRENTLY BEING HELD.", EditorStyles.helpBox);
            }

            if (myTarget.instantiatedPrefab != null)
            {
                EditorGUILayout.ObjectField("The instantiated prefab is: ", myTarget.instantiatedPrefab, typeof(GameObject), true);
            }
            


            /*if (myTarget.interactionObject.rightHoldPosition != null)
            {
                EditorGUILayout.ObjectField("Right Hold Transform:", myTarget.interactionObject.rightHoldPosition, typeof(Transform), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no right hold transform set for this object!", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.interactionObject.leftHoldPosition != null)
            {
                EditorGUILayout.ObjectField("Left Hold Transform:", myTarget.interactionObject.leftHoldPosition, typeof(Transform), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no left hold transform set for this object!", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.interactionObject.emptyParent != null)
            {
                EditorGUILayout.ObjectField("Parent Transform:", myTarget.interactionObject.emptyParent, typeof(Transform), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no hold transform set for this object!", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.interactionObject.dropPosition != null)
            {
                EditorGUILayout.LabelField("Drop Marker:", myTarget.interactionObject.dropPosition.name);
            }
            else
            {
                EditorGUILayout.LabelField("There is no drop marker set for this object.", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.interactionObject.rightHandTarget != null)
            {
                EditorGUILayout.ObjectField("Right Hand Target:", myTarget.interactionObject.rightHandTarget, typeof(Transform), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no right hand target set for this object.", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.interactionObject.leftHandTarget != null)
            {
                EditorGUILayout.ObjectField("Left Hand Target:", myTarget.interactionObject.leftHandTarget, typeof(Transform), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no left hand target set for this object.", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.linkedPrefab != null)
            {
                EditorGUILayout.ObjectField("Linked Prefab:", myTarget.linkedPrefab, typeof(GameObject), true);
            }

            GUILayout.Space(5);

            if (myTarget.interactionObject.holdSound != null)
            {
                EditorGUILayout.ObjectField("Hold Sound:", myTarget.interactionObject.holdSound, typeof(AudioClip), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no audio clip assigned for holding.", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.interactionObject.dropSound != null)
            {
                EditorGUILayout.ObjectField("Drop Sound:", myTarget.interactionObject.dropSound, typeof(AudioClip), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no audio clip assigned for dropping.", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.interactionObject.interactionSound != null)
            {
                EditorGUILayout.ObjectField("Interaction Sound:", myTarget.interactionObject.interactionSound, typeof(AudioClip), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no audio clip assigned for interaction.", EditorStyles.helpBox);
            }

            if (myTarget.interactionObject.inventorySound != null)
            {
                EditorGUILayout.ObjectField("Inventory Sound:", myTarget.interactionObject.inventorySound, typeof(AudioClip), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no audio clip assigned for inventory.", EditorStyles.helpBox);
            }*/



        }

    }
