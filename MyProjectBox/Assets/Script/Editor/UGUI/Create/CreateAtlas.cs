using UnityEngine;
using System.Collections;
using UnityEditor;
using Roma;

namespace Roma
{
    /// <summary>
    /// 生成图集
    /// </summary>
    [ExecuteInEditMode]
    public class CreateAtlas : MonoBehaviour
    {
        [MenuItem("Assets/UGUI/Create/Atlas #A")]
        static public void Create()
        {
            GetAtlasFile();
        }
        /// <summary>
        /// 点击由Texturepacker打成的图集，选择CreateAtlas，制作图集
        /// </summary>
        private static void GetAtlasFile()
        {
            string imagePath = "";      //被选择图集路径
            string cfgPath = "";        //被选择图集，由打包时生成伴随txt文件路径
            Object[] selection = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);//被选择的物品
            if (selection.Length == 0)
            {
                EditorUtility.DisplayDialog("没有选中图集PNG", "未选中任何图集", "确定");
                return;
            }
            imagePath = AssetDatabase.GetAssetPath(selection[0]);
            if (imagePath.Contains("panel_") || imagePath.Contains("icon_"))
            {
                //获取同文件夹下的字库配置信息
                cfgPath = imagePath.Replace(".png", ".txt");
                TextAsset myCfg = (TextAsset)AssetDatabase.LoadAssetAtPath(cfgPath, typeof(TextAsset));
                if (myCfg == null)
                {
                    EditorUtility.DisplayDialog("导出图集错误", "找不到图集配置文件" + selection[0].name + ".txt", "确定");
                    return;
                }
                CreateAtla(selection[0].name, imagePath, myCfg);
            }
            else
            {
                EditorUtility.DisplayDialog("导出图集错误", "请选择[图集]", "确定");
            }
        }
        private static void CreateAtla(string atlasName, string imagePath, TextAsset cfg)
        {
            //Debug.Log("该图集放在这个位置：" + imagePath);
            //Debug.Log("该图集相对的配置文件名：" + cfg.name);
            Texture2D image = (Texture2D)AssetDatabase.LoadAssetAtPath(imagePath, typeof(Texture2D));

            //获取的图集用Unity自带管理类制作Unity认识图集
            TextureImporter textureImporter = AssetImporter.GetAtPath(imagePath) as TextureImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Multiple;
            textureImporter.filterMode = FilterMode.Point;
            textureImporter.spritePackingTag = atlasName;
            textureImporter.mipmapEnabled = true;

            //从txt配置文件中读取图片信息
            UIAtlas atlas = new UIAtlas();
            JsonTool.LoadSpriteData(atlas, cfg);
            SpriteMetaData[] spriteArray = new SpriteMetaData[atlas.spriteList.Count];
            for (int i = 0; i < atlas.spriteList.Count; i++)
            {
                UIAtlas.Sprite spriteInfo = atlas.spriteList[i];
                SpriteMetaData metaData = new SpriteMetaData();
                metaData.name = spriteInfo.name;
                metaData.rect.x = spriteInfo.outer.x;
                metaData.rect.y = (float)image.height - spriteInfo.outer.y - spriteInfo.outer.height;
                metaData.rect.width = spriteInfo.outer.width;
                metaData.rect.height = spriteInfo.outer.height;
                //如果老数据中有内边距信息，那么就用老的边距信息
                SpriteMetaData oldMetaData = new SpriteMetaData();
                if (GetOldData(textureImporter, ref oldMetaData, spriteInfo.name))
                {
                    metaData.border = oldMetaData.border;
                    metaData.pivot = oldMetaData.pivot;
                }
                spriteArray[i] = metaData;
            }
            textureImporter.spritesheet = spriteArray;
            Debug.Log("图集创建成功！");
        }

        private static bool GetOldData(TextureImporter textureImporter, ref SpriteMetaData oldMetaData, string name)
        {
            foreach (SpriteMetaData item in textureImporter.spritesheet)
            {
                if (item.name.Equals(name))
                {
                    oldMetaData = item;
                    return true;
                }
            }
            return false;
        }
    }
}
