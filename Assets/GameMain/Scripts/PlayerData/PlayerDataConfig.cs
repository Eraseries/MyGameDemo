using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.Text;



[System.Serializable]
public class PlayerDataConfig 
{
    public float playerPositionX;
    public float playerPositionY;
    public int level;//冒险家等级
    public int coin;//冒险家金币
    public int diamond;//冒险家钻石
    public int energy;//冒险家能量
    public int exp;//冒险家经验
    public string playerName = "";//冒险家名字
    Dictionary<string,float> bag = new Dictionary<string, float>();//第一个参数物品index，第二个参数属性表
    public SettingDataConfig SettingData = new SettingDataConfig();//设置数据
}






