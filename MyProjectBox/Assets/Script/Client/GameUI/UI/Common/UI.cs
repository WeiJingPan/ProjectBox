using UnityEngine;
using System.Collections;

/// <summary>
/// 所有UI界面的基类
/// 打开关闭的记录，拖动等通用行为放这里
/// 注：UI子类全部以UIPanelXX命名
/// 节点名为小写panel_xx
/// </summary>
public class UI : MonoBehaviour 
{
    public Transform m_root;
    public GameObject m_panel;
    public GameObject m_btnClose;

    public bool m_isLoaded = false;

    public virtual void Init()
    {
        m_root = transform;
        Transform tempObj = m_root.Find("panel");
        if (tempObj != null)
        {
            m_panel = tempObj.gameObject;
        }
        tempObj = m_root.Find("panel/btn_close");
        if (tempObj != null)
        {
            m_btnClose = tempObj.gameObject;
        }
    }
    public void OnLoadUI()
    {
        if (m_isLoaded)
        {
            return;
        }
    }

}
