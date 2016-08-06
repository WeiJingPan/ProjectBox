using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using Roma;
using System.Text;


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

        //获取所有文字
        List<Text> txtList = new List<Text>();
        //GetText(transform,ref txtList);

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
            node.resId = GetResId(name);
            node.val = imageList[i].sprite.name;

            if (node.resId != 0)
            {
                m_dicPanelAllInfo.Add(node);
                imageList[i].sprite = null;
                if (!m_resList.Contains((uint)node.resId))m_resList.Add((uint)node.resId);
            }
            else
                Debug.Log("找不到资源：" + name);
        }
        for (int i = 0; i < txtList.Count; i++)
        {
            UINode node = new UINode();
            node.fullName = GetFullName(transform, txtList[i].transform);
            node.type = UINodeType.eFont;
            node.resId = GetResId(txtList[i].font.name.Trim());
            if (node.resId == 0) break;
            m_dicPanelAllInfo.Add(node);
            //将字库置空
            txtList[i].font = null;
            txtList[i].material = null;
            //保存资源到资源列表
            if (!m_resList.Contains((uint)node.resId)) m_resList.Add((uint)node.resId);
        }
    }
    public bool Load(Loaded loaded)
    {
        foreach (UINode item in m_dicPanelAllInfo)
        {
            if (item.type == UINodeType.eAtlas)
            {
                Transform trans = transform.FindChild(item.fullName);
                if (trans == null)
                {
                    Debug.Log("is null:" + item.fullName);
                    m_bLoaded = true;
                    m_isLoading = true;
                    loaded();
                    return false;
                }
                GameObject obj = trans.gameObject;

                //该节点为图集，请求图集
            }
        }
                return true;
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

    private string m_resCsvPath = Application.dataPath + "/Resource/resinfo.csv";
#if UNITY_EDITOR
    private int GetResId(string name)
    {
        AllResInfoCsv allResCsv = new AllResInfoCsv();
        if (!allResCsv.Load(m_resCsvPath, Encoding.Default))
        {
            Debug.LogError("ResInfo.csv 打开失败。");
            return 0;
        }
        if (!allResCsv.m_mapNameIDResInfo.ContainsKey(name))
        {
            EditorUtility.DisplayDialog("保存配置错误", "[" + name + "]并未添加到资源总表，无法获取资源id，保存配置失败！" + transform.name, "确定");
            return 0;
        }
        else return (int)allResCsv.m_mapNameIDResInfo[name].nResID;
    }
#endif
}
