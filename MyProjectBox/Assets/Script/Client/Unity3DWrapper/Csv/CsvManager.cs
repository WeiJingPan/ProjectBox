using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Roma
{
    public delegate CsvExWrapper CreateCSV();
    public class CsvManager : Singleton
    {
        public CsvManager() : base(true) { }
        public T GetCsv<T>(int key) where T : CsvExWrapper
        {
            CsvExWrapper csv;
            if (m_mapCSV.TryGetValue(key, out csv))
            {
                System.Object obj = csv;
                return (T)obj;
            }
            return default(T);
        }
        private Dictionary<int, CsvExWrapper> m_mapCSV = new Dictionary<int, CsvExWrapper>();
    }
}
