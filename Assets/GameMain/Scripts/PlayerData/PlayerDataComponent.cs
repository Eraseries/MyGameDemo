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

namespace StarForce
{
    public class PlayerDataComponent : GameFrameworkComponent
    {
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

        public PlayerDataConfig CreateSaveData()
        { //创建一个Save对象存储当前游戏数据
            PlayerDataConfig data = new PlayerDataConfig();
            data.coin = 10000;
            data.diamond = 10000;
            data.energy = 50;
            data.level = 1;
            data.role_exp = 0;
            return data;
        }

        private void Start()
        {
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
        public void SaveBySerialization(string text_name)
        {
            PlayerDataConfig save = CreateSaveData();
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
        private void LoadByDeserialization(string text_name)
        {
            if (File.Exists(Application.persistentDataPath + text_name))
            //判断文件是否创建
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open(Application.persistentDataPath + text_name, FileMode.Open);//打开文件
                PlayerDataConfig save = bf.Deserialize(fs) as PlayerDataConfig;
                //反序列化并将数据储存至save（因为返回变量类型不对，所以要强制转换为Save类
                //关文件流
                fs.Close();
                Log.Error(save.coins);
                SetPlayerData();
            }
            else
            {
                Debug.LogError("Data Not Found");
            }
        }

        //设置玩家保存的数据
        public void SetPlayerData()
        {
            //GameManager.Instance.coins = save.coins;
            //player.transform.position = new Vector2(save.playerPositionX, save.playerPositionY);
            //赋值
        }

        //操作
        public void Operate(string operate,int type = 1)
        {
            string text_name = "";
            switch (type)
            {
                case 1:
                    text_name = "/Data1.yj";
                    break;
                case 2:
                    text_name = "/Data2.yj";
                    break;
                case 3:
                    text_name = "/Data3.yj";
                    break;
                case 4:
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
    }
}
