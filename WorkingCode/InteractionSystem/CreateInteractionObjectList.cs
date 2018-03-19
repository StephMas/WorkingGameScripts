using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateInteractionObjectList
{
    [MenuItem("Assets/Create/Interaction Object List")]
    public static InteractionObjectList Create()
    {
        InteractionObjectList asset = ScriptableObject.CreateInstance<InteractionObjectList>();

        AssetDatabase.CreateAsset(asset, "Assets/InteractionObjectList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

    [MenuItem("Assets/Create/Interaction Object Data")]
    public static InteractionObjectData CreateData()
    {
        InteractionObjectData asset = ScriptableObject.CreateInstance<InteractionObjectData>();

        AssetDatabase.CreateAsset(asset, "Assets/SpaceDetective/ContactSystemObjects/ObjectReferences/InteractionObjectData.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}