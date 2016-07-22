using UnityEngine;
using System.Collections;

namespace Roma
{

    public enum eGameState
    {
        Login,
        Update,
        Game,
    }
    public class GlobalConfig
    {
        public static string s_gameServerIP = "";
        public static string s_gameServerPort = "";
        public static string s_fileServerIP = "";
        public static string s_fileServerPort = "";

        public static eDownLoadType m_downLoadType = eDownLoadType.WWW;
        public static eGameState m_gameState = eGameState.Login;

        public static int s_iVersion = -1;

        public const string s_configPath = "Config/";
        public const string s_allResInfoFile = "allresinfo.unity3d";
        public const string s_allCsvInfoFile = "allcsvinfo.unity3d";

        //实时阴影
        public static bool m_bRealTimeShadow = true;

        public static string GetGameServerPath()
        {
            return s_gameServerIP + ":" + s_gameServerPort;
        }

        public static string GetPlatform()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return "/android/";
                case RuntimePlatform.IPhonePlayer:
                    return "/ios/";
                case RuntimePlatform.WindowsWebPlayer:
                    return "/pc/";
                case RuntimePlatform.WindowsPlayer:
                    return "/pc/";
            }
            return "";
        }
        public static string GetFileServerBasePath()
        {
            if (m_downLoadType == eDownLoadType.WWW)
            {
                Security.PrefetchSocketPolicy(s_fileServerIP, 843);
                return "http://" + s_fileServerIP + ":" + s_fileServerPort + "/";
            }
            else if (m_downLoadType == eDownLoadType.StreamingAssetsPath)
            {
                return Application.streamingAssetsPath + GetPlatform();
            }
            else if (m_downLoadType == eDownLoadType.PersistentDataPath)
            {
                return "file://" + Application.persistentDataPath + GetPlatform();
            }
            return "";
        }
        public static string GetFileServerPath()
        {
            return "http://" + s_fileServerIP + ":" + s_fileServerPort + "/";
        }
    }
    public enum eDownLoadType
    {
        None,
        WWW,
        StreamingAssetsPath,
        PersistentDataPath,
    }
}
