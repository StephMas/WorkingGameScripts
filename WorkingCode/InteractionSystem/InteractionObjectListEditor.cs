using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using AC;

public class InteractionObjectListEditor : EditorWindow
{

    public InteractionObjectList interactionObjectList;
    private int viewIndex = 1;

    [MenuItem("Window/Interaction Object Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(InteractionObjectListEditor));
    }

    /*void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            interactionObjectList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(InteractionObjectList)) as InteractionObjectList;
        }

    }*/

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Inventory Object Editor", EditorStyles.largeLabel);

        interactionObjectList = EditorGUILayout.ObjectField("Object List:", interactionObjectList, typeof(InteractionObjectList), true) as InteractionObjectList;
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();

        if (interactionObjectList)
        {
            if (GUILayout.Button("Show Object List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = interactionObjectList;
            }
        }
        if (GUILayout.Button("Open Object List"))
        {
            OpenItemList();
        }

        GUILayout.EndHorizontal();


        if (interactionObjectList == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Object List", GUILayout.ExpandWidth(false)))
            {
                CreateNewItemList();
            }
            if (GUILayout.Button("Open Existing Object List", GUILayout.ExpandWidth(false)))
            {
                OpenItemList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (interactionObjectList)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < interactionObjectList.objectList.Count)
                {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Object", GUILayout.ExpandWidth(false)))
            {
                AddItem();
            }
            if (GUILayout.Button("Delete Object", GUILayout.ExpandWidth(false)))
            {
                DeleteItem(viewIndex - 1);
            }

            GUILayout.EndHorizontal();

            if (interactionObjectList.objectList == null)
            {
                return;
            }
                //Debug.Log("wtf");

            if (interactionObjectList.objectList.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Object:", viewIndex, GUILayout.ExpandWidth(false)), 1, interactionObjectList.objectList.Count);

                EditorGUILayout.LabelField("of   " + interactionObjectList.objectList.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                interactionObjectList.objectList[viewIndex - 1] = EditorGUILayout.ObjectField("Object Data:", interactionObjectList.objectList[viewIndex - 1], typeof(InteractionObjectData), true) as InteractionObjectData;

                InteractionObjectData dataStuff = interactionObjectList.objectList[viewIndex - 1];

                if (dataStuff.myObject)
                {
                    GUILayout.Space(10);
                    EditorGUILayout.Separator();
                    GUILayout.Space(10);

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.BeginVertical("Box");
                    GUILayout.Label(dataStuff.myObject.objectName, EditorStyles.boldLabel);
                    EditorGUILayout.ObjectField("Parent Object:", dataStuff.myObject.objectParent, typeof(Transform), true);
                    EditorGUILayout.ObjectField("Right Hold Position:", dataStuff.myObject.rHandPosition, typeof(Transform), true);
                    EditorGUILayout.ObjectField("Left Hold Position:", dataStuff.myObject.lHandPosition, typeof(Transform), true);
                    EditorGUILayout.ObjectField("Drop Position:", dataStuff.myObject.dropPosition, typeof(Transform), true);
                    EditorGUILayout.ObjectField("Right Target:", dataStuff.myObject.rHandTarget, typeof(Transform), true);
                    EditorGUILayout.ObjectField("Left Target:", dataStuff.myObject.lHandTarget, typeof(Transform), true);
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUILayout.Label("Sound FX", EditorStyles.boldLabel);
                    EditorGUILayout.ObjectField("Pick Up Sound:", dataStuff.myObject.pickUpSound, typeof(AudioClip), true);
                    EditorGUILayout.ObjectField("Drop Sound:", dataStuff.myObject.dropSound, typeof(AudioClip), true);
                    EditorGUILayout.ObjectField("Interaction Sound:", dataStuff.myObject.interactSound, typeof(AudioClip), true);
                    EditorGUILayout.ObjectField("Inventory Sound:", dataStuff.myObject.inventorySound, typeof(AudioClip), true);

                    GUILayout.Label("Inventory Properties", EditorStyles.boldLabel);
                    if (dataStuff.myObject.isInventoryItem)
                    {
                        GUILayout.Label("This is an inventory item.", EditorStyles.miniBoldLabel);
                        GUILayout.Label("Inventory Item: " + AC.KickStarter.inventoryManager.items[dataStuff.myObject.invID].label, EditorStyles.miniBoldLabel);
                        EditorGUILayout.ObjectField("Prefab Reference:", AC.KickStarter.inventoryManager.items[dataStuff.myObject.invID].linkedPrefab, typeof(GameObject), true);
                    }
                    else
                    {
                        GUILayout.Label("This is not an inventory item.", EditorStyles.miniBoldLabel);
                    }

                    GUILayout.EndVertical();
                }





            }
            else
            {
                GUILayout.Label("This Object List is Empty.");
                return;
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(interactionObjectList);
        }
    }

    void CreateNewItemList()
    {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        viewIndex = 1;
        interactionObjectList = CreateInteractionObjectList.Create();
        if (interactionObjectList)
        {
            interactionObjectList.objectList = new List<InteractionObjectData>();
            string relPath = AssetDatabase.GetAssetPath(interactionObjectList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenItemList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Interaction Object List", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            interactionObjectList = AssetDatabase.LoadAssetAtPath(relPath, typeof(InteractionObjectList)) as InteractionObjectList;
            if (interactionObjectList.objectList == null)
                interactionObjectList.objectList = new List<InteractionObjectData>();
            if (interactionObjectList)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddItem()
    {
        InteractionObjectData newData = CreateInteractionObjectList.CreateData();        
        if (newData)
        {

            string relPath = AssetDatabase.GetAssetPath(newData);
            EditorPrefs.SetString("ObjectPath", relPath);
            interactionObjectList.objectList.Add(newData);
            viewIndex = interactionObjectList.objectList.Count;
           
        }
    }

    void DeleteItem(int index)
    {
        interactionObjectList.objectList.RemoveAt(index);
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
