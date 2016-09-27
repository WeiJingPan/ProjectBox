using UnityEngine;
using System.Collections;

namespace Roma
{
    public class Client : MonoBehaviour
    {
        public static Client m_client = null;
        public string s_gameServerIP = "";
        public string s_gameServerPort = "";
        public string s_fileServerIP = "";
        public string s_fileServerPort = "";

        public bool m_bSingle = true;
        public bool m_bOcc = false;

        public UIPanelInitRes m_uiInitRes;

        public static Client Inst()
        {
            if (m_client == null)
            {
                m_client = GameObject.Find("game").GetComponent<Client>();
                m_client.InitIP();
                m_client.InitUI();
            }
            return m_client;
        }

        private void InitIP()
        {
            if (m_bSingle)
            {
                if(PlayerPrefs.GetString("Single","True").Equals("False"))
                {
                    m_bSingle = false;
                }
                else
                {
                    m_bSingle = true;
                }
            }
            if (string.IsNullOrEmpty(s_gameServerIP))
            {
                s_gameServerIP = PlayerPrefs.GetString("GameServerIP", s_gameServerIP);
                s_gameServerPort = PlayerPrefs.GetString("GameServerPort", s_gameServerPort);
            }
            if (string.IsNullOrEmpty(s_fileServerIP))
            {
                s_fileServerIP = PlayerPrefs.GetString("FileServerIp", s_fileServerIP);
                s_gameServerPort = PlayerPrefs.GetString("FileServerPort", s_fileServerPort);
            }
            GlobalConfig.s_gameServerIP = s_gameServerIP;
            GlobalConfig.s_gameServerPort = s_gameServerPort;
            GlobalConfig.s_fileServerIP = s_fileServerIP;
            GlobalConfig.s_fileServerPort = s_fileServerPort;
        }
        private void InitUI()
        {
            m_uiInitRes = GameObject.Find("panel_init_res").AddComponent<UIPanelInitRes>();
            m_uiInitRes.Init();
        }
    }
}
