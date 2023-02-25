using System.Collections.Generic;

namespace StarForce
{
    [System.Serializable]
    //冒险家数据
    public class PlayerDataConfig
    {
        public int level = 1;//冒险家等级
        public int coin = 1000;//冒险家金币
        public int diamond = 0;//冒险家钻石
        public int energy = 50;//冒险家能量
        public int exp = 0;//冒险家经验
        public int cur_select_map = 1; //当前所学的地图，默认第一个


        static System.Random random = new System.Random();
        public string playerName = "冒险家#" + random.Next(10000000, 99999999);//冒险家名字


        /// <summary>
        /// 关卡信息，场景ID---大关卡---小关卡
        /// </summary>
        //public Dictionary<string, Dictionary<int, int>> Stage = new Dictionary<string, Dictionary<int, int>>();
        public Dictionary<int, StageData> Stage = new Dictionary<int, StageData>();

        /// <summary>
        /// 第一个字典key是哪种背包。
        /// 第二个字典key物品在当前的格子位置
        /// </summary>
        public Dictionary<string, Dictionary<int, BagData>> Bag = new Dictionary<string, Dictionary<int, BagData>>();


        /// <summary>
        /// 拥有的角色以及角色数据，模型id,角色数据
        /// </summary>
        public Dictionary<int, RoleData> RoleBag = new Dictionary<int, RoleData>();

    }

    [System.Serializable]
    //角色数据
    public class RoleData
    {
        public int battle_pos = -1;  //-1为不出战，1234对应场景位置
        public int round_use_card_count = 1;   //每回合战斗可以使用多少张卡牌
        public int exp = 0; //角色经验
        public int level = 1; //角色等级
        public int index = -1; //角色Index 用来获取配置表的信息
        public int base_hp = 0; //角色基础血量
        public int extra_hp = 0; //额外血量加成
        public int total_hp = 0;//角色总血量
        public int priority = -1; //角色出战优先级
        public int rarity = -1; //角色稀有度
        public string type = ""; //角色类型
        public int dead_offer_exp = 0;//死亡提供经验
        //卡包
        //卡牌id,卡牌数据
        public Dictionary<int, CardData> CardBag = new Dictionary<int, CardData>();
    }

    [System.Serializable]
    //卡牌数据
    public class CardData
    {
        public int bag_pos; //背包里的位置
        public bool load_battle; //是否出战
        public int level; //卡牌等级
        public int exp; //卡牌经验
        public int index; //卡牌index  用来获取配置表的信息
    }

    [System.Serializable]
    //背包数据
    public class BagData
    {
        public string guid; //物品唯一id
        public int grid_index; //格子位置
        public int count; //物品数量
        public int index; //物品index，用来获取配置表的信息
    }

    [System.Serializable]
    //关卡数据
    public class StageData
    {
        public int big_stage = 1;
        public int small_stage = 1;
    }
}
