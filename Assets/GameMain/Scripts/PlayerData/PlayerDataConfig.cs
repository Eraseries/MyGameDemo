using System.Collections.Generic;
using UnityEngine;

namespace StarForce
{
    [System.Serializable]

    //冒险家数据
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


        /// <summary>
        /// 第一个字典key是哪种背包。
        /// 第二个字典key物品在当前的格子位置
        /// </summary>
        public Dictionary<string, Dictionary<int, BagData>> Bag = new Dictionary<string, Dictionary<int, BagData>>();

        /// <summary>
        /// 拥有的角色以及角色数据，模型id,角色数据
        /// </summary>
        public Dictionary<int,RoleData> RoleBag = new Dictionary<int,RoleData>();
    }

    //角色数据
    public class RoleData
    {
        public int battle_pos = -1;  //-1为不出战，1234对应场景位置
        public int round_use_card_count = 1;   //每回合战斗可以使用多少张卡牌
        public int exp; //角色经验

        //卡包
        //卡牌id,卡牌数据
        public Dictionary<int, CardData> CardBag = new Dictionary<int, CardData>(); 
    }

    //卡牌数据
    public class CardData
    {
        public int bag_pos; //背包里的位置
        public bool load_battle; //是否出战
        public int level; //卡牌等级
        public int exp; //卡牌经验
        public int index; //卡牌index  用来获取配置表的信息
    }

    //背包数据
}    public class BagData
    {
        public string guid; //物品唯一id
        public int grid_index; //格子位置
        public int count; //物品数量
        public int index; //物品index，用来获取配置表的信息
    }
