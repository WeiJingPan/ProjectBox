using UnityEngine;
using System.Collections;

namespace Roma
{
    public enum eAllCSV
    {
        eAC_icon,
    }
    public class LogicSystem
    {
        public LogicSystem() { }

        public void InitModule()
        {
            CsvManager.Inst = new CsvManager();
        }
    }
}
