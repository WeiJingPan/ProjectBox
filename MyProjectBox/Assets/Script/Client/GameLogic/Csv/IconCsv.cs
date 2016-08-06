using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Roma
{
    public enum eIconCsv
    {
        iconID,
        atlasResID,
        iconName,
        tips,
    }
    public class IconCsvData
    {
        /// <summary>
        /// 图标ID
        /// </summary>
        public int iconID;
        /// <summary>
        /// 图集资源ID
        /// </summary>
        public uint atlasResID;
        /// <summary>
        /// 图标名称
        /// </summary>
        public string iconName;
        /// <summary>
        /// 备注
        /// </summary>
        public float tips;
    }
    public class IconCsv : CsvExWrapper
    {
        public static CsvExWrapper CreateCSV()
        {
            return new IconCsv();
        }

        protected override void _Load()
        {
            for (int i = 0; i < m_csv.GetRows(); i++)
            {
                IconCsvData data = new IconCsvData();
                data.iconID = m_csv.GetIntData(i, (int)eIconCsv.iconID);
                data.atlasResID = (uint)m_csv.GetIntData(i, (int)eIconCsv.atlasResID);
                data.iconName = m_csv.GetData(i, (int)eIconCsv.iconName);
                data.tips = m_csv.GetFloatData(i, (int)eIconCsv.tips);
                m_mapData.Add(data.iconID, data);
            }
        }
        public IconCsvData GetData(int csvID)
        {
            IconCsvData data;
            if (m_mapData.TryGetValue(csvID, out data)) return data;
            return null;
        }
        public Dictionary<int, IconCsvData> m_mapData = new Dictionary<int, IconCsvData>();
    }
}
