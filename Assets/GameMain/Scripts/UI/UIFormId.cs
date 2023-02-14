//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace StarForce
{
    /// <summary>
    /// 界面编号。
    /// </summary>
    public enum UIFormId : byte
    {
        Undefined = 0,

        /// <summary>
        /// 弹出框。
        /// </summary>
        DialogForm = 1,

        /// <summary>
        /// 主菜单。
        /// </summary>
        MenuForm = 100,

        /// <summary>
        /// 设置。
        /// </summary>
        SettingForm = 101,

        /// <summary>
        /// 设置。
        /// </summary>
        SettingUI = 102,

        /// <summary>
        /// 关于。
        /// </summary>
        AboutForm = 103,

        /// <summary>
        /// 消息盒。
        /// </summary>
        MessageBoxUI = 104,

        /// <summary>
        /// 初始游戏界面。
        /// </summary>
        StartUI = 105,

        /// <summary>
        /// 加载进度条界面。
        /// </summary>
        LoadingUI = 106,

        /// <summary>
        /// 聊天界面。
        /// </summary>
        ChatUI = 107,

        /// <summary>
        /// 主界面。
        /// </summary>
        MainUI = 200,

        /// <summary>
        /// 玩家信息。
        /// </summary>
        PlayerInfoUI = 205,


        /// <summary>
        /// 背包。
        /// </summary>
        BagUI = 210,

        /// <summary>
        /// 仓库。
        /// </summary>
        RoleUI = 215,

        /// <summary>
        /// 商城。
        /// </summary>
        ShopUI = 220,


        /// <summary>
        /// 活动。
        /// </summary>
        LuckyDrawUI = 230,

        /// <summary>
        /// 战斗。
        /// </summary>
        SmallStageUI = 240,


        /// <summary>
        /// 战斗。
        /// </summary>
        BigStageUI = 245,

        /// <summary>
        /// 战斗。
        /// </summary>
        BattleUI = 250,
    }
}
