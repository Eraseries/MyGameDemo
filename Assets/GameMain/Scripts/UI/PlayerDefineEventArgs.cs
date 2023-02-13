//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
namespace StarForce
{
    /// <summary>
    /// 玩家自定义订阅事件。
    /// </summary>
    public sealed class PlayerDefineEventArgs : GameEventArgs
    {
        public enum EventType
        {
            UpdatePlayerData,
        }
        public static readonly int EventId = typeof(PlayerDefineEventArgs).GetHashCode();
        public override int Id => EventId;


        /// <summary>
        /// 这里执行要更新的数据
        /// </summary>
        /// <param name="eventType">传入事件类型</param>
        /// <returns></returns>
        public PlayerDefineEventArgs DefineEvent(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.UpdatePlayerData:
                    UpdatePlayerData();
                    return this;
                default:
                    return this;
            }
        }

        void UpdatePlayerData()
        {
            //Log.Error("更新玩家数据");
            //PlayerDataConfig playerDataConfig = GameEntry.PlayerData.GetPlayerData();
        }

        public override void Clear()
        {
            //throw new System.NotImplementedException();
        }


        /// <summary>
        /// 使用教程
        /// GameEntry.Event.Fire(this, ReferencePool.Acquire<PlayerDefineEventArgs>().DefineEvent(PlayerDefineEventArgs.EventType.UpdatePlayerData));
        /// GameEntry.Event.Subscribe(PlayerDefineEventArgs.EventId, TestEvent);
        ///         public void TestEvent(object sender, GameEventArgs e)
        ///         {
        ///         Debug.LogError("更新数据成功，执行UI展示方法");
        ///         }
        /// GameEntry.Event.Unsubscribe(PlayerDefineEventArgs.EventId, TestEvent);
        /// </summary>


    }
}
