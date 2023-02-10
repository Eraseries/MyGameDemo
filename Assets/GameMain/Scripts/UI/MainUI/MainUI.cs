using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
/// <summary>
/// 主界面模块
/// </summary>
namespace StarForce
{
    public class MainUI : UGuiForm
    {
        private Transform content;
        private Transform bottom;
        private Transform right;
        private Transform left;


        Button houseBtn;
        Button bagBtn;
        Button cardBtn;
        Button clanBtn;
        Button stageBtn;
        Button playerInfoBtn;

        Button[] buttons;

        [HideInInspector]
        public Button battleBtn;

        Button chatBtn;
        Button settingBtn;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "MainUI";


            content = transform.Find("Background");

            bottom = content.Find("Bottom");
            houseBtn = bottom.Find("HouseBtn").GetComponent<Button>();
            bagBtn = bottom.Find("BagBtn").GetComponent<Button>();
            cardBtn = bottom.Find("CardBtn").GetComponent<Button>();
            clanBtn = bottom.Find("ClanBtn").GetComponent<Button>();
            stageBtn = bottom.Find("StageBtn").GetComponent<Button>();
            battleBtn = bottom.Find("BattleBtn").GetComponent<Button>();


            left = content.Find("Left");
            playerInfoBtn = left.Find("PlayerInfo/Level").GetComponent<Button>();

            right = content.Find("Right");
            chatBtn = right.Find("ChatBtn").GetComponent<Button>();
            settingBtn = right.Find("SettingBtn").GetComponent<Button>();

            AddBtnEvent(houseBtn, () =>
            {
                //AssignUISprite("Image.spriteatlas", "", bottom.Find("StageBtn/Icon").GetComponent<Image>()); 
                GameEntry.UI.OpenUIForm(UIFormId.HouseUI);
            });
            AddBtnEvent(bagBtn, () => { GameEntry.UI.OpenUIForm(UIFormId.BagUI); });
            AddBtnEvent(cardBtn, () =>
            {
                if (GetUIForm("BagUI") != null)
                {
                    GetUIForm("BagUI").Open();
                }
            });
            AddBtnEvent(clanBtn, () => { });
            AddBtnEvent(stageBtn, () =>
            {
                GameEntry.UI.OpenUIForm(UIFormId.BigStageUI);
            });
            AddBtnEvent(battleBtn, () =>
            {
                GameEntry.UI.OpenUIForm(UIFormId.BattleUI);
            });
            AddBtnEvent(chatBtn, () => { });
            AddBtnEvent(settingBtn, () => { GameEntry.UI.OpenUIForm(UIFormId.SettingUI); });
            AddBtnEvent(playerInfoBtn, () => { GameEntry.UI.OpenUIForm(UIFormId.PlayerInfoUI); });
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

    }

}