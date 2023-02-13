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
    public int level;//角色等级
    public int coin;//角色金币
    public int diamond;//角色钻石
    public int energy;//角色能量
    public float role_exp;//角色经验
    public string playerName;//角色名字
    Dictionary<string,float> bag = new Dictionary<string, float>();//第一个参数物品index，第二个参数属性表
}






