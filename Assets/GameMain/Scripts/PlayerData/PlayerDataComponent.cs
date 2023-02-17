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
    public class PlayerDataComponent : GameFrameworkComponent
    {
        
        /// <summary>
        /// 玩家数据
        /// </summary>
        private PlayerDataConfig PlayerData
        {
            get;
            set;
        }


        /// <summary>
        /// 保存到哪个存档
        /// </summary>
        private SaveDataType CurrentSaveType
        {
            get;
            set;
        }


        /// <summary>
        /// 保存到哪个存档
        /// </summary>
        public enum SaveDataType    //枚举类型
        {
            Data1,
            Data2,
            Data3,
            Data4,
        }
        //定义名为colorDic的字典，<>内存放两种类型，表示两者为一一对应关系
        private Dictionary<SaveDataType, Sprite> colorDic;

        [System.Serializable]           //在unity面板上序列化显示
        public struct saveData   //写成结构体的形式
        {
            public SaveDataType color;
            public Sprite sprite;
            public string save_name;
        }
        public saveData[] saveDatas;  //结构体数组


        /// <summary>
        /// 创建新的玩家数据
        /// </summary>
        /// <returns></returns>
        public PlayerDataConfig CreateNewPlayerData()
        {
            CurrentSaveType = SaveDataType.Data1;
            //创建一个Save对象存储当前游戏数据
            PlayerDataConfig data = new PlayerDataConfig();
            System.Random random = new System.Random();
            data.coin = 1000;
            data.diamond = 0;
            data.energy = 50;
            data.playerName = "冒险家#" + random.Next(10000000, 99999999);
            data.level = 1;
            data.curStage = 1;
            data.exp = 0;
            return data;
        }

        protected override void Awake()
        {
            base.Awake();
            Operate("delete");
        }


        private void Start()
        {
            if (!Operate("load"))
            {
                PlayerData = CreateNewPlayerData();
                Operate("save");
            }

            LoadSettingData();


            colorDic = new Dictionary<SaveDataType, Sprite>();
            for (int i = 0; i < saveDatas.Length; i++)
            {
                //字典中是否有了此种颜色
                if (!colorDic.ContainsKey(saveDatas[i].color))
                {
                    colorDic.Add(saveDatas[i].color, saveDatas[i].sprite);
                    //压入字典中
                }
            }
        }


        private void LoadSettingData()
        {
            if (PlayerPrefs.GetString("MuteSound", "no_set") == "no_set")
            {
                PlayerPrefs.SetString("MuteSound", "false");
                PlayerPrefs.SetFloat("SoundValue", 0.75f);
            }
            
        }

        //将数据保存到文本里
        public bool SaveBySerialization(string text_name = "/Data1.txt",bool force_save = false)
        {
            if (File.Exists(Application.persistentDataPath + text_name))
            //判断文件是否创建
            {
                if(force_save)
                {
                    save(text_name);
                }
                return false;
            }
            else
            {
                save(text_name);
                return true;
            }


        }

        private void save(string text_name)
        {
            BinaryFormatter bf = new BinaryFormatter();
            //创建一个二进制形式
            FileStream fs = File.Create(Application.persistentDataPath + text_name);
            //这里指使用持久路径创建一个文件流并将其保存在Data.txt里（具体在哪就不打了，反正创建了）
            //由于持久路径在Windows系统是隐藏的，所以无法找到Data.txt本身
            //如果想看到，可以改成dataPath(就像下文json的代码里一样)
            //文件后缀可以随便改，甚至是自定义的（比如我这里用了txt）
            bf.Serialize(fs, PlayerData);
            //将Save对象转化为字节
            fs.Close();
            //把文件流关了
        }





        //加载数据
        private bool LoadByDeserialization(string text_name = "/Data1.txt")
        {
            if (File.Exists(Application.persistentDataPath + text_name))
            //判断文件是否创建
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open(Application.persistentDataPath + text_name, FileMode.Open);//打开文件
                this.PlayerData = bf.Deserialize(fs) as PlayerDataConfig;
                //反序列化并将数据储存至save（因为返回变量类型不对，所以要强制转换为Save类
                //关文件流
                fs.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool DeleteByDeserialization(string text_name = "/Data1.txt")
        {
            if (File.Exists(Application.persistentDataPath + text_name))
            //判断文件是否创建
            {
                File.Delete(Application.persistentDataPath + text_name);
                return true;
            }
            else
            {
                return false;
            }
        }

        //存档/删除操作(外部接口)
        public bool Operate(string operate,bool force_save = false)
        {
            string text_name = "";
            switch (CurrentSaveType)
            {
                case SaveDataType.Data1:
                    text_name = "/Data1.txt";
                    break;
                case SaveDataType.Data2:
                    text_name = "/Data2.txt";
                    break;
                case SaveDataType.Data3:
                    text_name = "/Data3.txt";
                    break;
                case SaveDataType.Data4:
                    text_name = "/Data4.txt";
                    break;
                default:
                    text_name = "/Data1.txt";
                    break;
            }
            if (operate == "save")
            {
                return SaveBySerialization(text_name, force_save);
            }
            else if(operate == "load")
            {
                return LoadByDeserialization(text_name);
            }
            else if(operate == "delete")
            {
                return DeleteByDeserialization(text_name);
            }
            else
            {
                Log.Error("操作错误");
                return false;
            }
        }

        public PlayerDataConfig GetPlayerData()
        {
            return PlayerData;
        }

        public void SetPlayerData(PlayerDataConfig temp_playerData)
        {
            PlayerData = temp_playerData;
            Operate("save",true);
        }

        //保存到哪个存档
        private void SetSaveType(SaveDataType saveDataType)
        {
            CurrentSaveType = saveDataType;
        }
    }
}
