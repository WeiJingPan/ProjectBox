using UnityEngine;
using System.Collections;
using Roma;
using System.Collections.Generic;

public struct SingleName
{
    public static string m_CSV = "csv";
}
public class SingletonManager 
{
    public static SingletonManager Inst = new SingletonManager();
    private Dictionary<string, Singleton> m_mapSingleton = new Dictionary<string, Singleton>();
}
