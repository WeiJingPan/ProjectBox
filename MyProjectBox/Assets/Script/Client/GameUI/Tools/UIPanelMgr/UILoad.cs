using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Roma;

public class UILoadResMgr
{
    private static Dictionary<uint, AssetBundle> m_mapUIAllAB = new Dictionary<uint, AssetBundle>();
    public static bool ContainRes(uint resID)
    {
        return m_mapUIAllAB.ContainsKey(resID);
    }

    public static void AddABToMap(uint resID, AssetBundle ab)
    {
        if (m_mapUIAllAB.ContainsKey(resID)) return;
        m_mapUIAllAB.Add(resID, ab);
    }
    public static AssetBundle GetABByID(uint resID)
    {
        if (m_mapUIAllAB.ContainsKey(resID)) return m_mapUIAllAB[resID];
        return null;
    }
    public static void Destroy(){}

    public static Dictionary<uint, List<UILoad>> m_loadChecheList = new Dictionary<uint, List<UILoad>>();
}

public class UILoad : MonoBehaviour 
{
    public uint m_resID = 0;
    public string m_value = "";

    public virtual void Load(int iconID)
    {
        if (iconID == 0) return;
        //IconCsv iconCsv=CsvManager.Inst.GetCsv<IconCsv>((int))
    }
}
