using UnityEngine;
using System.Collections;

namespace Roma
{
    public enum ResType
    {
        eRT_UnKnown=-1,         //自定义资源
        eRT_FirstBlood=0,       //全局的首要通信资源
        eRT_CsvList=1,          //全局的csv列表文件
        eRT_ResInfos=2,         //全局的资源信息
        eRT_Model=3,            //模型
        eRT_Tex=4,              //纹理
        eRT_Bone=5,             //蒙皮骨骼
        eRT_Effect=6,           //特效
        eRT_Terrain=7,          //地形
        eRT_SceneCfg=8,         //地图文件配置
        eRT_Material=9,         //材质
        eRT_LightMap=10,        //光照贴图
        eRT_SkinnedComponent=11,//蒙皮组件
        eRT_AttackComponent=12, //挂点组件
        eRT_Sound=13,           //声音
        eRT_UI=14,              //UI
        eRT_AnimationClip=15,   //动画剪辑
        eRT_SceneAnim=16,       //场景剧情动画
    }
    public class ResInfo
    {
        public void Save(ref LusuoStream f)
        {
            string strRef;
            strRef = Type2String(iType);
            f.WriteString(ref strRef);
            f.WriteString(ref strUrl);
            f.WriteString(ref strName);
            f.WriteInt(iVersion);

            f.WriteInt(iSize);
            f.WriteUInt(nResID);
            f.WriteUInt((uint)cacheflags);
            f.WriteString(ref md5);
        }
        public static ResType String2Type(string strName)
        {
            if (strName == "resinfo") return ResType.eRT_ResInfos;
            else if (strName == "csv") return ResType.eRT_CsvList;
            else if (strName == "模型") return ResType.eRT_Model;
            else if (strName == "纹理") return ResType.eRT_Tex;
            else if (strName == "骨骼") return ResType.eRT_Bone;
            else if (strName == "地形") return ResType.eRT_Terrain;
            else if (strName == "地图配置") return ResType.eRT_SceneCfg;
            else if (strName == "特效") return ResType.eRT_Effect;
            else if (strName == "材质") return ResType.eRT_Material;
            else if (strName == "光照贴图") return ResType.eRT_LightMap;
            else if (strName == "蒙皮组件") return ResType.eRT_SkinnedComponent;
            else if (strName == "挂点组件") return ResType.eRT_AttackComponent;
            else if (strName == "声音") return ResType.eRT_Sound;
            else if (strName == "UI") return ResType.eRT_UI;
            else if (strName == "动画") return ResType.eRT_AnimationClip;
            else if (strName == "场景动画") return ResType.eRT_SceneAnim;
            return ResType.eRT_UnKnown;
        }
        public static string Type2String(ResType type)
        {
            switch (type)
            {
                case ResType.eRT_ResInfos: return "resinfo";
                case ResType.eRT_CsvList: return "csv";
                case ResType.eRT_Model: return "模型";
                case ResType.eRT_Tex: return "纹理";
                case ResType.eRT_Bone: return "骨骼";
                case ResType.eRT_Terrain: return "地形";
                case ResType.eRT_SceneCfg: return "地图配置";
                case ResType.eRT_Effect: return "特效";
                case ResType.eRT_LightMap: return "光照贴图";
                case ResType.eRT_Material: return "材质";
                case ResType.eRT_SkinnedComponent: return "蒙皮组件";
                case ResType.eRT_AttackComponent: return "挂点组件";
                case ResType.eRT_Sound: return "声音";
                case ResType.eRT_UI: return "UI";
                case ResType.eRT_AnimationClip: return "动画";
                case ResType.eRT_SceneAnim: return "场景动画";

                default: return "自定义资源";
            }

        }

        public string strUrl;
        public string strName;
        public int iVersion;
        public ResType iType;
        public int iSize;
        public uint cacheflags;     //是否先尝试读取缓存
        public uint nResID;         //资源ID  //程序用索引ID
        public string md5;          //资源扩展信息
    }
}
