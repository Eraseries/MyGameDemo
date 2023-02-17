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
    public class BattleStartUI : UGuiForm
    {
        private Transform content;
        private Transform top;
        private float timer = 5;
        [HideInInspector]
        public Button backBtn;
        BattlePanel battlePanel;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "BattleStartUI";
            battlePanel = new BattlePanel();
            battlePanel.Init();
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
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                Close(true);
                timer = 5;
                GameEntry.UI.OpenUIForm(UIFormId.BattleUI);
                GameEntry.Sound.PlayMusic(5);
            }
        }

    }

}