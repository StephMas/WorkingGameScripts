using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleInteractionSystem))]
public class SimpleInteractionCharacterEditor : Editor
{

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

        GUILayout.Space(10);

        if (myTarget.interactionObject == null)
        {
            EditorGUILayout.LabelField("THERE IS NO INTERACTION OBJECT SET.", EditorStyles.miniBoldLabel);
        }
        else
        {
            EditorGUILayout.LabelField("OBJECT INFORMATION", EditorStyles.miniBoldLabel);

            GUILayout.Space(10);

 
            if (myTarget.objectBeingHeld == true)
            {
                EditorGUILayout.LabelField("THIS OBJECT IS CURRENTLY BEING HELD.", EditorStyles.helpBox);
            }

            if (myTarget.instantiatedPrefab != null)
            {
                EditorGUILayout.ObjectField("The instantiated prefab is: ", myTarget.instantiatedPrefab, typeof(GameObject), true);
            }

            if (myTarget.rightHoldPosition != null)
            {
                EditorGUILayout.ObjectField("Right Hold Transform:", myTarget.rightHoldPosition, typeof(Transform), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no right hold transform set for this object!", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.leftHoldPosition != null)
            {
                EditorGUILayout.ObjectField("Left Hold Transform:", myTarget.leftHoldPosition, typeof(Transform), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no left hold transform set for this object!", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.emptyParent != null)
            {
                EditorGUILayout.ObjectField("Parent Transform:", myTarget.emptyParent, typeof(Transform), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no hold transform set for this object!", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.dropMarker != null)
            {
                EditorGUILayout.LabelField("Drop Marker:", myTarget.dropMarker.name);
            }
            else
            {
                EditorGUILayout.LabelField("There is no drop marker set for this object.", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.rightHandTarget != null)
            {
                EditorGUILayout.ObjectField("Right Hand Target:", myTarget.rightHandTarget, typeof(Transform), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no right hand target set for this object.", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.leftHandTarget != null)
            {
                EditorGUILayout.ObjectField("Left Hand Target:", myTarget.leftHandTarget, typeof(Transform), true);
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

            if (myTarget.holdSound != null)
            {
                EditorGUILayout.ObjectField("Hold Sound:", myTarget.holdSound, typeof(AudioClip), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no audio clip assigned for holding.", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.dropSound != null)
            {
                EditorGUILayout.ObjectField("Drop Sound:", myTarget.dropSound, typeof(AudioClip), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no audio clip assigned for dropping.", EditorStyles.helpBox);
            }

            GUILayout.Space(5);

            if (myTarget.interactionSound != null)
            {
                EditorGUILayout.ObjectField("Interaction Sound:", myTarget.interactionSound, typeof(AudioClip), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no audio clip assigned for interaction.", EditorStyles.helpBox);
            }

            if (myTarget.inventorySound != null)
            {
                EditorGUILayout.ObjectField("Inventory Sound:", myTarget.inventorySound, typeof(AudioClip), true);
            }
            else
            {
                EditorGUILayout.LabelField("There is no audio clip assigned for inventory.", EditorStyles.helpBox);
            }



        }

    }
}