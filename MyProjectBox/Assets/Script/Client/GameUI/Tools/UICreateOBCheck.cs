using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using System;

public class UICreateOBCheck : Editor
{

    [MenuItem("Tools/ModifyPrefab")]
    static void Modify()
    {
        ModifyPrefab<Image>(delegate(Component obj)
        {
            Image img = obj as Image;
            img.raycastTarget = false;
        });
    }
    public static void ModifyPrefab<T>(Action<Component> callBack) where T : MonoBehaviour
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            T[] scripts = obj.GetComponentsInChildren<T>(true);
            for (int i = 0; i < scripts.Length; i++)
            {
                callBack(scripts[i]);
            }
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
//#if UNITY_EDITOR
//    void Reset()
//    {
//        Debug.Log("Reset重置这个启动了");
//    }
//    void OnValidate()
//    {
//        Debug.Log("OnValidate启动了这个");
//    }
//#endif
}
