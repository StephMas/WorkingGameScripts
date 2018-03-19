using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AC;

[CustomEditor(typeof(SimpleInteractionObject))]
public class SimpleInteractionObjectEditor : Editor
{

    public override void OnInspectorGUI()
    {
        SimpleInteractionObject myTarget = (SimpleInteractionObject)target;
        
        GUILayout.Space(20);

        EditorGUILayout.LabelField("OBJECT INFORMATION", EditorStyles.boldLabel);

        GUILayout.Space(10);

        myTarget.rightHoldPosition = (Transform)EditorGUILayout.ObjectField("RIGHT HOLD TRANSFORM", myTarget.rightHoldPosition, typeof(Transform), true);

        GUILayout.Space(5);

        myTarget.leftHoldPosition = (Transform)EditorGUILayout.ObjectField("LEFT HOLD TRANSFORM", myTarget.leftHoldPosition, typeof(Transform), true);

        GUILayout.Space(5);

        myTarget.dropPosition = (Marker)EditorGUILayout.ObjectField("DROP MARKER", myTarget.dropPosition, typeof(Marker), true);

        GUILayout.Space(5);

        myTarget.emptyParent = (Transform)EditorGUILayout.ObjectField("EMPTY PARENT", myTarget.emptyParent, typeof(Transform), true);

        GUILayout.Space(5);



        myTarget.rightHandTarget = (Transform)EditorGUILayout.ObjectField("RIGHT HAND TARGET:", myTarget.rightHandTarget, typeof(Transform), true);

        GUILayout.Space(5);

        myTarget.leftHandTarget = (Transform)EditorGUILayout.ObjectField("LEFT HAND TARGET:", myTarget.leftHandTarget, typeof(Transform), true);

        GUILayout.Space(5);

        myTarget.holdSound = (AudioClip)EditorGUILayout.ObjectField("HOLD SOUND:", myTarget.holdSound, typeof(AudioClip), true);

        GUILayout.Space(5);

        myTarget.dropSound = (AudioClip)EditorGUILayout.ObjectField("DROP SOUND:", myTarget.dropSound, typeof(AudioClip), true);

        GUILayout.Space(5);

        myTarget.interactionSound = (AudioClip)EditorGUILayout.ObjectField("INTERACTION SOUND:", myTarget.interactionSound, typeof(AudioClip), true);

        GUILayout.Space(15);

        myTarget.inventorySound = (AudioClip)EditorGUILayout.ObjectField("INVENTORY SOUND:", myTarget.inventorySound, typeof(AudioClip), true);

        GUILayout.Space(15);

        if (myTarget.inventorySound == null || myTarget.leftHoldPosition == null || myTarget.rightHoldPosition == null || myTarget.dropPosition == null || myTarget.rightHandTarget == null || myTarget.leftHandTarget == null || myTarget.holdSound == null || myTarget.dropSound == null || myTarget.interactionSound == null)
        {
            EditorGUILayout.LabelField("This object is missing the following assignments:", EditorStyles.whiteBoldLabel);
        }

        GUILayout.Space(10);

        if (myTarget.rightHoldPosition == null)
        {
            EditorGUILayout.LabelField("There is no right hold transform defined.", EditorStyles.helpBox);
        }

        if (myTarget.leftHoldPosition == null)
        {
            EditorGUILayout.LabelField("There is no left hold transform defined.", EditorStyles.helpBox);
        }

        if (myTarget.emptyParent == null)
        {
            EditorGUILayout.LabelField("There is no parent transform defined.", EditorStyles.helpBox);
        }

        if (myTarget.dropPosition == null)
        {
            EditorGUILayout.LabelField("There is no drop marker defined.", EditorStyles.helpBox);
        }

        if (myTarget.rightHandTarget == null)
        {
            EditorGUILayout.LabelField("There is no right hand target defined.", EditorStyles.helpBox);
        }

        if (myTarget.leftHandTarget == null)
        {
            EditorGUILayout.LabelField("There is no left hand target defined.", EditorStyles.helpBox);
        }

        if (myTarget.holdSound == null)
        {
            EditorGUILayout.LabelField("There is no audio clip assigned for 'HOLD'.", EditorStyles.helpBox);
        }

        if (myTarget.dropSound == null)
        {
            EditorGUILayout.LabelField("There is no audio clip assigned for 'DROP'.", EditorStyles.helpBox);
        }

        if (myTarget.interactionSound == null)
        {
            EditorGUILayout.LabelField("There is no audio clip assigned for interaction.", EditorStyles.helpBox);
        }

        if (myTarget.inventorySound == null)
        {
            EditorGUILayout.LabelField("There is no audio clip assigned for inventory.", EditorStyles.helpBox);
        }

    }
}