//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace StarForce
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }

        public static HPBarComponent HPBar
        {
            get;
            private set;
        }

        /// 获取玩家数据。
        public static PlayerDataComponent PlayerData
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取Json数据配置表。
        /// </summary>
        public static ConfigDataComponent ConfigData
        {
            get;
            private set;
        }
        private static void InitCustomComponents()
        {
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            HPBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent>();
            PlayerData = UnityGameFramework.Runtime.GameEntry.GetComponent<PlayerDataComponent>();
            ConfigData = UnityGameFramework.Runtime.GameEntry.GetComponent<ConfigDataComponent>();
        }
    }
}
