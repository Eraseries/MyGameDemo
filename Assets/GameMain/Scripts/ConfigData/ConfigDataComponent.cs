//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DG.Tweening.Core.Easing;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using LitJson;
using System.Collections;

namespace StarForce
{
    public class ConfigDataComponent : GameFrameworkComponent
    {
        string[] json_name_table =
        {
            "pet_config_new",
        };
        //存储所有模块的配置表数据
        Dictionary<string, Dictionary<string, Dictionary<string, object>>> data_config = new Dictionary<string, Dictionary<string, Dictionary<string, object>>>();


        private void Start()
        {
            foreach (var item in json_name_table)
            {
                LoadData(item);
            }
        }
        public void LoadData(string json_name = "")
        {
            string FileName = "Assets/GameMain/ExcelTable/JsonTable/"+ json_name + ".json";
            try
            {
                StreamReader json = File.OpenText(FileName);
                string input = json.ReadToEnd();
                Dictionary<string, Dictionary<string, object>> jsonObject = JsonMapper.ToObject<Dictionary<string, Dictionary<string, object>>>(input);
                data_config[json_name] = jsonObject;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }
        //ConfigData具体逻辑
        void Test(string table_name)
        {
            if (!data_config.ContainsKey(table_name))
            {
                return;
            }
            Dictionary<string, Dictionary<string, object>> jsonObject = data_config[table_name];
            foreach (var item in jsonObject)
            {
                if (item.Key == "0")
                {
                    foreach (var list in item.Value)
                    {
                        string temp_key = list.Key;
                        object temp_value_type = list.Value;
                        string[] str = temp_key.Split('#');
                        Debug.LogError(str[0]);
                    }
                }
                else
                {

                    //foreach (var list in item.Value)
                    //{
                    //    Debug.LogError(list);
                    //}
                }
            }
        }

        void Test2()
        {
           // Di arrayList = new ArrayList();
           // arrayList.
        }

    }
}
