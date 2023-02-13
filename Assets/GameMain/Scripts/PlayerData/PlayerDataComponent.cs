﻿//------------------------------------------------------------
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
        private PlayerDataConfig Data
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
            data.coin = random.Next(5000, 10000);
            data.diamond = random.Next(1, 300);
            data.energy = random.Next(1, 100);
            data.level = 1;
            data.role_exp = 0;
            return data;
        }

        protected override void Awake()
        {

        }

        private void Start()
        {
            if (!LoadByDeserialization())
            {
                Data = CreateNewPlayerData();
            }
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

        //将数据保存到文本里
        public void SaveBySerialization(string text_name = "/Data1.yj")
        {
            PlayerDataConfig save = CreateNewPlayerData();
            //获取当前的游戏数据存在Save对象里
            BinaryFormatter bf = new BinaryFormatter();
            //创建一个二进制形式
            FileStream fs = File.Create(Application.persistentDataPath + text_name);
            //这里指使用持久路径创建一个文件流并将其保存在Data.yj里（具体在哪就不打了，反正创建了）
            //由于持久路径在Windows系统是隐藏的，所以无法找到Data.yj本身
            //如果想看到，可以改成dataPath(就像下文json的代码里一样)
            //文件后缀可以随便改，甚至是自定义的（比如我这里用了yj）
            bf.Serialize(fs, save);
            //将Save对象转化为字节
            fs.Close();
            //把文件流关了
        }

        //加载数据
        private bool LoadByDeserialization(string text_name = "/Data1.yj")
        {
            if (File.Exists(Application.persistentDataPath + text_name))
            //判断文件是否创建
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open(Application.persistentDataPath + text_name, FileMode.Open);//打开文件
                Data = bf.Deserialize(fs) as PlayerDataConfig;
                //反序列化并将数据储存至save（因为返回变量类型不对，所以要强制转换为Save类
                //关文件流
                fs.Close();
                return true;
            }
            else
            {
                Debug.LogError("加载数据失败，创建一个新的数据存档");
                return false;
            }
        }

        //存档/删除操作(外部接口)
        public void Operate(string operate,SaveDataType saveDataType = SaveDataType.Data1)
        {
            string text_name = "";
            switch (saveDataType)
            {
                case SaveDataType.Data1:
                    text_name = "/Data1.yj";
                    break;
                case SaveDataType.Data2:
                    text_name = "/Data2.yj";
                    break;
                case SaveDataType.Data3:
                    text_name = "/Data3.yj";
                    break;
                case SaveDataType.Data4:
                    text_name = "/Data4.yj";
                    break;
            }
            if (operate == "save")
            {
                SaveBySerialization(text_name);
            }
            else
            {
                LoadByDeserialization(text_name);
            }
        }

        public PlayerDataConfig GetPlayerData()
        {
            Log.Error(Data.coin);
            return Data;
        }


        private void DoDestroy()
        {
            Operate("save",CurrentSaveType);
        }

        //保存到哪个存档
        private void SetSaveType(SaveDataType saveDataType)
        {
            CurrentSaveType = saveDataType;
        }
    }
}
