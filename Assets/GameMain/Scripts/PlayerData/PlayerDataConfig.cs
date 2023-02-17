using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
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
        public int curStage;//当前关卡
        public string playerName = "";//冒险家名字
        public SettingDataConfig SettingData = new SettingDataConfig();//设置数据


        /// <summary>
        /// 第一个字典key是哪种背包。
        /// 第二个字典key物品在当前的格子位置
        /// </summary>
        public Dictionary<string, Dictionary<int, BagDataConfig>> Bag = new Dictionary<string, Dictionary<int, BagDataConfig>>();

        /// <summary>
        /// 拥有的所有角色，int为模型id   bool该模型是否出战
        /// </summary>
        public Dictionary<int, bool> Role = new Dictionary<int, bool>();

        /// <summary>
        /// 角色出战队列，int 为模型id   int为出战位置
        /// </summary>
        public Dictionary<int, int> BattleQueue = new Dictionary<int, int>();
    }
}

