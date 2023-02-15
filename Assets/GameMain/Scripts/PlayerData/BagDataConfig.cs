using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.Text;



[System.Serializable]
public class BagDataConfig
{
    public string guid; //物品唯一id
    public int grid_index; //格子位置
    public int count; //物品数量
    public int index; //物品index，用来获取配置表的信息
}





