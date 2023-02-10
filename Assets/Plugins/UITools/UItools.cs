using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using LitJson;

public class UItools : MonoBehaviour
{

#if UNITY_EDITOR
    [MenuItem("Assets/Texture/GenerateSprites")]
    public static void GenerateSprites()
    {
        Texture2D tex = Selection.activeObject as Texture2D;
        if (tex == null)
        {
            return;
        }
        tex.alphaIsTransparency = true;
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.alphaIsTransparency = true;
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        path = Application.dataPath.Replace("Assets", "") + path;
        int i = path.LastIndexOf("/");
        path = path.Substring(0, i + 1) + tex.name + ".txt";
        if (!File.Exists(path))
        {
            Debug.LogError(string.Format("File not exist:{0}", path));
        }
        TextureImporter assetImp = null;
        Dictionary<string, Vector4> tIpterMap = new Dictionary<string, Vector4>();
        assetImp = GetTextureIpter(tex);
        SaveBoreder(tIpterMap, assetImp);
        string json_txt = File.ReadAllText(path);
        JsonData json_data = JsonMapper.ToObject(json_txt);
        WriteMeta(json_data, tex, tIpterMap);
        File.Delete(path);
        AssetDatabase.Refresh();
    }

    //如果这张图集已经拉好了9宫格，需要先保存起来
    static void SaveBoreder(Dictionary<string, Vector4> tIpterMap, TextureImporter tIpter)
    {
        for (int i = 0, size = tIpter.spritesheet.Length; i < size; i++)
        {
            tIpterMap.Add(tIpter.spritesheet[i].name, tIpter.spritesheet[i].border);
        }
    }
    static TextureImporter GetTextureIpter(Texture2D texture)
    {
        TextureImporter textureIpter = null;
        string impPath = AssetDatabase.GetAssetPath(texture);
        textureIpter = TextureImporter.GetAtPath(impPath) as TextureImporter;
        return textureIpter;
    }
    //写信息到SpritesSheet里
    static void WriteMeta(JsonData json_data, Texture2D tex, Dictionary<string, Vector4> borders)
    {
        var assetImp = GetTextureIpter(tex);
        JsonData json_frames = json_data["frames"];
        SpriteMetaData[] metaData = new SpriteMetaData[json_frames.Count];
        Debug.Log(string.Format("WriteMeta:{0}", json_frames.Count));
        int count = 0;
        foreach (var key in json_frames.Keys)
        {
            int i = key.LastIndexOf(".");
            string name = key.Substring(0, i);
            Rect rect = new Rect();
            var value = json_frames[key]["frame"];
            rect.width = int.Parse(value["w"].ToString());
            rect.height = int.Parse(value["h"].ToString());
            rect.x = int.Parse(value["x"].ToString());
            rect.y = tex.height - int.Parse(value["y"].ToString()) - rect.height;
            metaData[count].rect = rect;
            metaData[count].pivot = new Vector2(0.5f, 0.5f);
            metaData[count].name = name;
            if (borders.ContainsKey(name))
            {
                metaData[count].border = borders[name];
            }
            ++count;
        }
        assetImp.spritesheet = metaData;
        assetImp.textureType = TextureImporterType.Sprite;
        assetImp.spriteImportMode = SpriteImportMode.Multiple;
        assetImp.mipmapEnabled = false;

        // iphone 默认作用RGBA16
        TextureImporterPlatformSettings textureSetting = assetImp.GetPlatformTextureSettings("iPhone");
        if (!textureSetting.overridden)
        {
            textureSetting.name = "iPhone";
            textureSetting.overridden = true;
            textureSetting.format = TextureImporterFormat.RGBA16;
            assetImp.SetPlatformTextureSettings(textureSetting);
        }

        assetImp.SaveAndReimport();
    }
#endif


}
