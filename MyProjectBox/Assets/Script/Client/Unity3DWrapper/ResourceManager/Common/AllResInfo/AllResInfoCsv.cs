using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Roma
{
    enum eCSV_Enum
    {
        eCE_ResID,
        eCE_ResName,
        eCE_Size,
        eCE_Version,
        eCE_Url,
        eCE_Type,
        eCE_CacheFlags,
        eCE_ResMd5,
    }
    public class AllResInfoCsv:CsvExWrapper
    {
        protected override void _Save()
        {
            //将m_listResInfo通过ID排序
            base._Save();
            m_listResInfo.Sort((x,y)=>
            {
                if (x.nResID == y.nResID) return 0;
                else if (x.nResID > y.nResID) return 1;
                else return -1;
            });
            foreach (ResInfo key in m_listResInfo)
            {
                int nRow = m_csv.AddRow();
                m_csv.SetData(nRow, (int)eCSV_Enum.eCE_ResID, key.nResID.ToString());
                m_csv.SetData(nRow, (int)eCSV_Enum.eCE_ResName, key.strName);
                m_csv.SetData(nRow, (int)eCSV_Enum.eCE_Size, key.iSize.ToString());
                m_csv.SetData(nRow, (int)eCSV_Enum.eCE_Version, key.iVersion.ToString());
                m_csv.SetData(nRow, (int)eCSV_Enum.eCE_Url, key.strUrl);
                m_csv.SetData(nRow, (int)eCSV_Enum.eCE_Type,ResInfo.Type2String(key.iType));
                m_csv.SetData(nRow, (int)eCSV_Enum.eCE_CacheFlags, key.cacheflags.ToString());
                m_csv.SetData(nRow, (int)eCSV_Enum.eCE_ResMd5, key.md5.ToString());
            }
        }
        public override void Clear()
        {
            base.Clear();
        }
        protected override void _Load()
        {
            for (int i = 0; i < m_csv.GetRows(); i++)
            {
                ResInfo res = new ResInfo();
                res.nResID = (uint)m_csv.GetIntData(i, (int)eCSV_Enum.eCE_ResID);
                res.strName = m_csv.GetData(i, (int)eCSV_Enum.eCE_ResName);
                res.iSize = m_csv.GetIntData(i, (int)eCSV_Enum.eCE_Size);
                res.iVersion = m_csv.GetIntData(i, (int)eCSV_Enum.eCE_Version);
                res.strUrl = m_csv.GetData(i, (int)eCSV_Enum.eCE_Url);
                res.iType = ResInfo.String2Type( m_csv.GetData(i, (int)eCSV_Enum.eCE_Type));
                res.cacheflags = (uint)m_csv.GetIntData(i, (int)eCSV_Enum.eCE_CacheFlags);
                res.md5 = m_csv.GetData(i, (int)eCSV_Enum.eCE_ResMd5);

                m_listResInfo.Add(res);
                m_mapNameIDResInfo[res.strName] = res;
                m_mapComIDResInfo[res.nResID] = res;
            }
        }
        public Dictionary<string, ResInfo> m_mapNameIDResInfo = new Dictionary<string, ResInfo>();
        public Dictionary<uint, ResInfo> m_mapComIDResInfo = new Dictionary<uint, ResInfo>();
        public List<ResInfo> m_listResInfo = new List<ResInfo>();
        public static Dictionary<uint, ResInfo> CheckNewVersion(Dictionary<uint, ResInfo> oldCsv, Dictionary<uint, ResInfo> newCsv)
        {
            if (oldCsv == null || newCsv == null) return null;
            foreach (KeyValuePair<uint, ResInfo> oldItem in oldCsv)
            {
                ResInfo newInfo = null;
                //遍历老的资源，如果新资源中有
                string oldFilePath = Application.persistentDataPath + "/android" + oldItem.Value.strUrl;
                if (newCsv.TryGetValue(oldItem.Key, out newInfo))
                {
                    if (newInfo.nResID == 0) newInfo.bNewVersion = false;
                    else if (File.Exists(oldFilePath))
                    {
                        //方式1：通过文件对比更新，端游常用
                        //MD5一样，那就是不更新的，支持断开续更
                        //byte[] bytes=File.ReadAllBytes(oldFilePath);
                        //string oldFileMD5=MD5.GetMd5Hash(bytes);
                        //if(oldFileMD5==newInfo.md5)
                        //{
                        //      newInfo.bNewVersion=false;
                        //}
                        //方式2:通过配置更新，手游常用
                        if (oldItem.Value.md5 == newInfo.md5 && oldItem.Value.iVersion == newInfo.iVersion && oldItem.Value.strName == newInfo.strName)
                            newInfo.bNewVersion = false;
                        else if (!oldItem.Value.strName.Equals(newInfo.strName))
                        {
                            //删除情况1：
                            //该id的资源名称修改了，之前的老资源就要删除掉
                            Debug.Log("1.del old res.." + oldFilePath);
                            if (File.Exists(oldFilePath)) File.Delete(oldFilePath);
                        }
                    }
                    else
                    {
                        //删除情况2：
                        //在老版本中有，新版本中无的，就是垃圾文件，删除掉
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                            Debug.Log("2.del rubbish res"+oldFilePath);
                        }
                    }
                }
            }
            return newCsv;
        }
    }
}