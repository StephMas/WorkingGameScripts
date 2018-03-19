using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using AC;

//This is to hold a reference to the interaction object and tie in to all of the data

public class InteractionObjectData : ScriptableObject

{
    [SerializeField]
    public InteractionObject myObject; 

    [CustomEditor(typeof(InteractionObjectData))]
    public class InteractionObjectDataEditor : Editor
    {     

        public override void OnInspectorGUI()
        {
            InteractionObjectData myTarget = (InteractionObjectData)target;

            myTarget.myObject = (InteractionObject)CustomGUILayout.ObjectField<InteractionObject>("Object:", myTarget.myObject, true);

            if (myTarget.myObject == null)
            {
                GUILayout.Label("Please choose an Interaction Object to continue.");
                return;
            }
            else
            {
                myTarget.myObject.interactionConstantID = ObjectFieldToID(myTarget.myObject.interactionObject, myTarget.myObject.interactionConstantID);
                myTarget.myObject.interactionObject = ObjectIDToField(myTarget.myObject.interactionObject, myTarget.myObject.interactionConstantID);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.BeginVertical("Box");
                GUILayout.Label(myTarget.myObject.objectName, EditorStyles.boldLabel);
                EditorGUILayout.ObjectField("Parent Object:", myTarget.myObject.objectParent, typeof(Transform), true);
                EditorGUILayout.ObjectField("Right Hold Position:", myTarget.myObject.rHandPosition, typeof(Transform), true);
                EditorGUILayout.ObjectField("Left Hold Position:", myTarget.myObject.lHandPosition, typeof(Transform), true);
                EditorGUILayout.ObjectField("Drop Position:", myTarget.myObject.dropPosition, typeof(Transform), true);
                EditorGUILayout.ObjectField("Right Target:", myTarget.myObject.rHandTarget, typeof(Transform), true);
                EditorGUILayout.ObjectField("Left Target:", myTarget.myObject.lHandTarget, typeof(Transform), true);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.Label("Sound FX", EditorStyles.boldLabel);
                EditorGUILayout.ObjectField("Pick Up Sound:", myTarget.myObject.pickUpSound, typeof(AudioClip), true);
                EditorGUILayout.ObjectField("Drop Sound:", myTarget.myObject.dropSound, typeof(AudioClip), true);
                EditorGUILayout.ObjectField("Interaction Sound:", myTarget.myObject.interactSound, typeof(AudioClip), true);
                EditorGUILayout.ObjectField("Inventory Sound:", myTarget.myObject.inventorySound, typeof(AudioClip), true);

                GUILayout.Label("Inventory Properties", EditorStyles.boldLabel);
                if (myTarget.myObject.isInventoryItem)
                {
                    GUILayout.Label("This is an inventory item.", EditorStyles.miniBoldLabel);
                    GUILayout.Label("Inventory Item: " + AC.KickStarter.inventoryManager.items[myTarget.myObject.invID].label, EditorStyles.miniBoldLabel);
                    EditorGUILayout.ObjectField("Prefab Reference:", AC.KickStarter.inventoryManager.items[myTarget.myObject.invID].linkedPrefab, typeof(GameObject), true);
                }
                else
                {
                    GUILayout.Label("This is not an inventory item.", EditorStyles.miniBoldLabel);
                }


                GUILayout.EndVertical();
            }

        }
        public static int FieldToID<T>(T field, int _constantID) where T : Transform
        {
            if (field == null)
            {
                return _constantID;
            }

            if (field.GetComponent<ConstantID>())
            {
                if (!field.gameObject.activeInHierarchy && field.GetComponent<ConstantID>().constantID == 0)
                {
                    field.GetComponent<ConstantID>().AssignInitialValue(true);
                }
                _constantID = field.GetComponent<ConstantID>().constantID;
            }
            else
            {
                field.gameObject.AddComponent<ConstantID>();
                _constantID = field.GetComponent<ConstantID>().AssignInitialValue(true);
                AssetDatabase.SaveAssets();
            }

            return _constantID;
        }

        public static T IDToField<T>(T field, int _constantID) where T : Transform
        {
            T newField = field;
            if (_constantID != 0)
            {
                newField = Serializer.returnComponent<T>(_constantID);

                if (newField != null)
                {
                    field = newField;
                }

                EditorGUILayout.BeginVertical("Button");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Recorded ConstantID: " + _constantID.ToString(), EditorStyles.miniLabel);

                if (field == null)
                {
                    if (GUILayout.Button("Search scenes", EditorStyles.miniButton))
                    {
                        AdvGame.FindObjectWithConstantID(_constantID);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            return field;
        }

        public static int ObjectFieldToID(GameObject field, int _constantID)
        {
            if (field == null)
            {
                return _constantID;
            }

            if (field.GetComponent<ConstantID>())
            {
                if (!field.gameObject.activeInHierarchy && field.GetComponent<ConstantID>().constantID == 0)
                {
                    field.GetComponent<ConstantID>().AssignInitialValue(true);
                }
                _constantID = field.GetComponent<ConstantID>().constantID;
            }
            else
            {
                field.gameObject.AddComponent<ConstantID>();
                _constantID = field.GetComponent<ConstantID>().AssignInitialValue(true);
                AssetDatabase.SaveAssets();
            }

            return _constantID;
        }

        public GameObject ObjectIDToField(GameObject field, int _constantID)
        {
            if (_constantID != 0)
            {
                ConstantID newID = Serializer.returnComponent<ConstantID>(_constantID);
                if (field != null && field.GetComponent<ConstantID>() != null && field.GetComponent<ConstantID>().constantID == _constantID)
                { }
                else if (newID != null && !Application.isPlaying)
                {
                    field = newID.gameObject;
                }

                EditorGUILayout.BeginVertical("Button");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Recorded ConstantID: " + _constantID.ToString(), EditorStyles.miniLabel);
                if (field == null)
                {
                    if (GUILayout.Button("Search scenes", EditorStyles.miniButton))
                    {
                        AdvGame.FindObjectWithConstantID(_constantID);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }
            return field;
        }

    }

}
