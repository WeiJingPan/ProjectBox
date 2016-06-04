using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[ExecuteInEditMode]
public class ExportAtlas : MonoBehaviour 
{
    [MenuItem("Assets/UGUI/Export/Atlas &A")]
    public static void ExportIconU3D()
    {
        GetPngDirectory();
    }

    private static void GetPngDirectory()
    {
        //获取当前选中的png文件夹     
        string pngPath = "";
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        for (int i = 0; i < selection.Length; i++)
        {
            if (selection[i].name.Contains("panel_") || selection[i].name.Contains("icon_"))
            {
                pngPath = AssetDatabase.GetAssetPath(selection[i]);
                BuildU3D(pngPath);
            }
        }
    }

    private static void BuildU3D(string pngPath)
    {
        //最新代码时删除这里
        List<Sprite> spriteList = new List<Sprite>();
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(pngPath);
        foreach (Object obj in assets)
        {
            spriteList.Add(obj as Sprite);
        }
        string u3dPath = Application.dataPath + "/" + pngPath.Replace("Assets/Resource", "StreamingAsset/" +ExportDefine.m_prefix + "/Resource");
        string strNewPath = Path.GetDirectoryName(u3dPath);

        //如果采用最新打包，只启用这段代码即可。
        strNewPath = strNewPath.Replace("\\", "/");
        Directory.CreateDirectory(strNewPath);

        string targetPath = "";
        //目标路径，最新代码时删除这里
        if (u3dPath.Contains(".png"))
        {
            targetPath = u3dPath.Replace(".png", ".unity3d");
        }
        else if (u3dPath.Contains(".jpg"))
        {
            targetPath = u3dPath.Replace(".jpg", ".unity3d");
        }
        if (spriteList.Count == 0)
            return;

        Debug.Log("strNewPath:" + strNewPath);
        BuildPipeline.BuildAssetBundle(null, spriteList.ToArray(), targetPath, BuildAssetBundleOptions.CollectDependencies,ExportDefine.m_buildTarget);
        Debug.Log("打包图集成功！");
    }

}
