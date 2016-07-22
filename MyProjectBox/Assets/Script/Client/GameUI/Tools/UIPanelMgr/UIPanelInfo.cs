using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;


public enum UINodeType
{
    eNone = 0,
    eAtlas,
    eFont,
}
public class UINode
{
    public string fullName;  // 全名称
    public UINodeType type; // 类型
    public int resId;      // 对应的资源id
    public string val;      // 对应的值，文字不用
}
public class UIPanelInfo : MonoBehaviour 
{
    //资源列表，用于下载时的进度显示
    public List<uint> m_resList = new List<uint>();
    //将信息存储在组件上
    public List<UINode> m_dicPanelAllInfo = new List<UINode>();

    private bool m_isLoading = false;
    public delegate void Loaded();
    private Loaded m_loaded;
    private bool m_bLoaded = true;

    public void Sava()
    {
        m_dicPanelAllInfo.Clear();
        //获取所有图片
        List<Image> imageList = new List<Image>();
        GetImage(transform, ref imageList);

        for (int i = 0; i < imageList.Count; i++)
        {
            UINode node = new UINode();
            node.fullName = GetFullName(transform, imageList[i].transform);
            string spritePath=AssetDatabase.GetAssetPath(imageList[i].sprite.GetInstanceID());
            TextureImporter textureImporter = AssetImporter.GetAtPath(spritePath) as TextureImporter;
            node.type = UINodeType.eAtlas;
            if (null == textureImporter)
            {
                continue;
            }
            //通过名称获取资源id
            int start = spritePath.LastIndexOf("/") + 1;
            int end = spritePath.LastIndexOf(".");

            string name = spritePath.Substring(start, end - start);
            
        }
    }
    private void GetImage(Transform trans, ref List<Image> list)
    {
        Image image = trans.GetComponent<Image>();
        if (image != null && image.sprite != null)
        {
            list.Add(image);
        }
        for (int i = 0; i < trans.childCount; i++)
        {
            GetImage(trans.GetChild(i), ref list);
        }
    }
    private string GetFullName(Transform parent, Transform curTrans)
    {
        string fullName = "";
        while (null != curTrans && curTrans != parent)
        {
            fullName = curTrans.name + "/" + fullName;
            curTrans = curTrans.parent;
        }
        return fullName.Substring(0, fullName.Length - 1);
    }
    private int GetResId(string name)
    {
        return new int();
    }
}
