using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Roma
{
    public class CsvEx
    {
        public enum eTypes
        {
            TypeUnknown,
            Typedouble,
            Typefloat,
            TypeInt,
            TypeUnit,
            TypeBool,
            TypeString,
            TypeEnum,
        }
        public CsvEx(){}

        ~CsvEx()
        {
            m_dt.Clear();
        }
        private bool CheckCSV()
        {
            if (m_dt.Count == 0)
            {
                return true;
            }
            for (int i = 0, j = m_dt.Count - 1; i < j; i++,j--)
            {
                if (m_dt[i].Count != m_dt[j].Count)
                {
                    Debug.LogWarningFormat("表格行数：{0}", m_dt.Count);

                    StringBuilder sb = new StringBuilder();
                    foreach (string s in m_dt[i])
                    {
                        sb.Append(s);
                        sb.Append('、');
                    }

                    Debug.LogWarningFormat("第{0}行：列数{1},内容：{2}", i, m_dt[i].Count, sb);

                    sb = new StringBuilder();
                    foreach (string s in m_dt[j])
                    {
                        sb.Append(s);
                        sb.Append('、');
                    }

                    Debug.LogWarningFormat("行数据格式：{0}", sb);

                    Debug.LogError("Csv文件有问题");

                    return false;
                }
            }
            return true;
        }
        private eTypes String2Type(ref string str)
        {
            if (str == "double")
            {
                return eTypes.Typedouble;
            }
            else if (str == "float")
            {
                return eTypes.Typefloat;
            }
            else if (str == "uint")
            {
                return eTypes.TypeUnit;
            }
            else if (str == "int")
            {
                return eTypes.TypeInt;
            }
            else if (str == "bool")
            {
                return eTypes.TypeBool;
            }
            else if (str == "string")
            {
                return eTypes.TypeString;
            }
            else if (str == "enum")
            {
                return eTypes.TypeEnum;
            }
            else
            {
                return eTypes.TypeUnknown;
            }
        }
        private string Type2String(eTypes t)
        {
            switch (t)
            {
                case eTypes.Typedouble:
                    return "double";
                case eTypes.Typefloat:
                    return "float";
                case eTypes.TypeInt:
                    return "int";
                case eTypes.TypeBool:
                    return "bool";
                case eTypes.TypeString:
                    return "string";
                case eTypes.TypeUnit:
                    return "uint";
                case eTypes.TypeEnum:
                    return "enum";
                default: return "";
            }
        }

        private bool NoSame(ref string[] strArr)
        {
            for (int i = 0; i < strArr.Length; i++)
            {
                for (int j = 0; j < strArr.Length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    if (strArr[i].Length == strArr[j].Length)
                    {
                        if (strArr[i] == strArr[j])
                        {
                            Debug.LogErrorFormat("第{0}列{1}和第{2}列{3}重复", i.ToString(), strArr[i], j.ToString(), strArr[j]);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public void Clear()
        {
            m_dt.Clear();
        }
        public int AddRow()
        {
            int iRow = m_dt.Count;
            m_dt.Add(new List<string>());
            for (int i = 0; i < m_strArrEName.Length; i++)
            {
                m_dt[iRow].Add("");
            }
            return iRow;
        }
        public void SetData(int uRow, int uCol, string data)
        {
            if (m_dt.Count > uRow)
            {
                if (m_dt[uRow].Count > uCol)
                {
                    m_dt[uRow][uCol] = data;
                }
            }
        }
        public float GetFloatData(int uRow, int uCol)
        {
            float fOut;
            if (m_dt.Count > uRow)
            {
                if (m_dt[uRow].Count > uCol)
                {
                    if (float.TryParse(m_dt[uRow][uCol], out fOut))
                    {
                        return fOut;
                    }
                }
            }
            return 0.0f;
        }
        public int GetIntData(int uRow, int uCol)
        {
            int iOut;
            if (m_dt.Count > uRow)
            {
                if (m_dt[uRow].Count > uCol)
                {
                    if (int.TryParse(m_dt[uRow][uCol], out iOut))
                    {
                        return iOut;
                    }
                    //m_dt.Rows[uRow]. Columns[uCol][uRow] = data;
                }
            }
            return 0;
        }

        public bool GetBoolData(int uRow, int uCol)
        {
            bool bOut;
            if (m_dt.Count > uRow)
            {
                if (m_dt[uRow].Count > uCol)
                {
                    if (bool.TryParse(m_dt[uRow][uCol], out bOut))
                    {
                        return bOut;
                    }
                    //m_dt.Rows[uRow]. Columns[uCol][uRow] = data;
                }
            }
            return false;
        }

        public string GetData(int uRow, int uCol)
        {
            if (m_dt.Count > uRow)
            {
                if (m_dt[uRow].Count > uCol)
                {
                    return m_dt[uRow][uCol];
                }
            }
            return "";
        }

        public T GetEnumData<T>(int uRow, int uCol)
        {
            string data = "";
            if (m_dt.Count > uRow)
            {
                if (m_dt[uRow].Count > uCol)
                {
                    data = m_dt[uRow][uCol];
                }
            }
            return (T)Enum.Parse(typeof(T), data);
        }

        public int GetRows() { return m_dt.Count; }

        private bool IsEmptyOrSpace(string s)
        {
            if (string.Empty == s)
                return true;
            foreach (char c in s)
            {
                if (c != ' ')
                    return false;
            }
            return true;
        }
        public void Save(LusuoStream lf, Encoding ecoding)
        {
            if (null == lf)
            {
                return;
            }

            StreamWriter sw = new StreamWriter(lf.GetStream(), ecoding);

            for (int i = 0; i < m_strArrEName.Length; i++)
            {
                m_strSaveData += m_strArrEName[i];
                if (i < m_strArrEName.Length - 1)
                {
                    m_strSaveData += ",";
                }
            }
            sw.WriteLine(m_strSaveData);
            m_strSaveData = "";

            for (int i = 0; i < m_strArrCName.Length; i++)
            {
                m_strSaveData += m_strArrCName[i];
                if (i < m_strArrCName.Length - 1)
                {
                    m_strSaveData += ",";
                }
            }
            sw.WriteLine(m_strSaveData);
            m_strSaveData = "";

            for (int i = 0; i < m_IndexType.Length; i++)
            {
                m_strSaveData += Type2String(m_IndexType[i]);
                if (i < m_strArrCName.Length - 1)
                {
                    m_strSaveData += ",";
                }
            }

            sw.WriteLine(m_strSaveData);
            m_strSaveData = "";

            for (int i = 0; i < m_dt.Count; i++)
            {
                if (IsEmptyOrSpace(m_dt[i][0]))
                {
                    continue;
                }

                for (int j = 0; j < m_dt[0].Count; j++)
                {
                    m_strSaveData += m_dt[i][j];
                    if (j < m_dt[0].Count - 1)
                    {
                        m_strSaveData += ",";
                    }
                }

                if (i < m_dt.Count - 1)
                {
                    sw.WriteLine(m_strSaveData);
                }
                else
                {
                    sw.Write(m_strSaveData);
                }

                m_strSaveData = "";
            }
            sw.Close();
        }
        public void Save(string strPath, Encoding encoding, bool bNewCreate)
        {
            if (string.IsNullOrEmpty(strPath))
            {
                return;
            }
            LusuoStream lf = null;
            try
            {
                if (bNewCreate)
                {
                    if (File.Exists(strPath))
                    {
                        File.Delete(strPath);
                    }

                    string strFullPath = Path.GetDirectoryName(strPath);

                    if (!Directory.Exists(strFullPath))
                    {
                        Directory.CreateDirectory(strFullPath);
                    }
                }

                lf = new LusuoStream(new FileStream(strPath, bNewCreate ? FileMode.CreateNew : FileMode.Truncate, FileAccess.Write));
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Cann't Open csv  errinfo=" + ex.Message);
                return;
            }
            Save(lf, encoding);

            lf.Close();
        }

        public bool Load(string strPath, Encoding encoding)
        {
            if (string.IsNullOrEmpty(strPath))
            {
                return false;
            }

            LusuoStream lf = null;
            try
            {
                lf = new LusuoStream(new FileStream(strPath, FileMode.Open));
            }
            catch (System.Exception)
            {
                return false;
            }
            bool bRef = Load(lf, encoding);
            lf.Close();
            return bRef;
        }

        public bool Load(LusuoStream lf, Encoding encoding)
        {
            if (null == lf)
            {
                return false;
            }

            if (lf.Length() <= 0)
            {
                return false;
            }

            StreamReader sr = new StreamReader(lf.GetStream(), encoding);
            string strEName = sr.ReadLine();
            if (string.IsNullOrEmpty(strEName))
            {
                return false;
            }
            m_strArrEName = strEName.Split(',');
            if (!NoSame(ref m_strArrEName))
            {
                Debug.LogError("表头第一行重复！读取失败!" + strEName);
                return false;
            }

            string strCName = sr.ReadLine();
            if (string.IsNullOrEmpty(strCName))
            {
                return false;
            }
            m_strArrCName = strCName.Split(',');
            if (!NoSame(ref m_strArrCName))
            {
                Debug.LogError("表头第二行重复！读取失败!");
                return false;
            }

            string strType = sr.ReadLine();
            if (string.IsNullOrEmpty(strType))
            {
                return false;
            }
            string[] strArrType = strType.Split(',');
            m_IndexType = new CsvEx.eTypes[strArrType.Length];
            for (int i = 0; i < strArrType.Length; i++)
            {
                m_IndexType[i] = String2Type(ref strArrType[i]);
            }

            string strTemp;
            while ((strTemp = sr.ReadLine()) != null)
            {
                List<string> listString = new List<string>();
                string[] strArr = strTemp.Split(',');

                if (strArr.Length > 0 && IsEmptyOrSpace(strArr[0]))
                {
                    continue;
                }

                for (int i = 0; i < strArr.Length; i++)
                {
                    listString.Add(strArr[i]);
                }
                m_dt.Add(listString);
            }

            if (!CheckCSV())
            {
                return false;
            }
            return true;
        }

        public CsvEx.eTypes GetType(uint _col)
        {
            return CsvEx.eTypes.TypeUnknown;
        }

        public void SetCNameENameType(string[] strCName, string[] strEName, eTypes[] eType)
        {
            m_strArrCName = strCName;
            m_strArrEName = strEName;
            m_IndexType = eType;
        }


        private List<List<string>> m_dt = new List<List<string>>();
        private string[] m_strArrCName;
        public string[] m_strArrEName;
        public string m_strSaveData;
        private eTypes[] m_IndexType = null;
    }
    public class CsvExWrapper
    {
        public CsvExWrapper(){}

        public void SetTitle(string[] strCName, string[] strEName)
        {

        }
    }
}
